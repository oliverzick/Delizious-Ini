namespace Delizious.Ini
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using IniParser.Model;
    using IniParser.Model.Configuration;
    using IniParser.Parser;

    internal sealed class IniParserAdapter : IIniDocument
    {
        private readonly IniData iniData;

        private IniParserAdapter(IniData iniData)
        {
            this.iniData = iniData;
        }

        public static IniParserAdapter CreateEmpty(IniDocumentConfiguration configuration)
        {
            using (var textReader = new StringReader(string.Empty))
            {
                return LoadFrom(textReader, configuration);
            }
        }

        public static IniParserAdapter LoadFrom(TextReader textReader, IniDocumentConfiguration configuration)
        {
            try
            {
                var parser = new IniDataParser(MakeIniParserConfiguration(configuration));
                var iniData = parser.Parse(textReader.ReadToEnd());

                return new IniParserAdapter(iniData);
            }
            catch (Exception exception)
            {
                throw PersistenceException.LoadingFailed(exception);
            }
        }

        private static IniParserConfiguration MakeIniParserConfiguration(IniDocumentConfiguration configuration)
            => new IniParserConfiguration
               {
                   AllowCreateSectionsOnFly = false,
                   AllowDuplicateKeys = configuration.DuplicatePropertyBehavior.Transform(new AllowDuplicateKeysTransformation()),
                   AllowDuplicateSections = configuration.DuplicateSectionBehavior.Transform(new AllowDuplicateSectionsTransformation()),
                   AllowKeysWithoutSection = false,
                   AssigmentSpacer = configuration.PropertyAssignmentSpacer.ToString(),
                   CaseInsensitive = configuration.CaseSensitivity.Transform(new CaseSensitivityTransformation()),
                   ConcatenateDuplicateKeys = false,
                   KeyValueAssigmentChar = configuration.PropertyAssignmentSeparator.ToChar(),
                   NewLineStr = configuration.NewlineString.ToString(),
                   OverrideDuplicateKeys = configuration.DuplicatePropertyBehavior.Transform(new OverrideDuplicateKeysTransformation()),
                   SectionStartChar = configuration.SectionBeginningDelimiter.ToChar(),
                   SectionEndChar = configuration.SectionEndDelimiter.ToChar(),
                   SectionRegex = MakeSectionRegex(configuration), // Needs to be specified after SectionStartChar and SectionEndChar to prevent recreation of SectionRegex with default pattern
                   SkipInvalidLines = configuration.InvalidLineBehavior.Transform(new InvalidLineBehaviorTransformation()),
                   ThrowExceptionsOnError = true
               };

        private static Regex MakeSectionRegex(IniDocumentConfiguration configuration)
        {
            const string whitespace = @"\p{Zs}*";
            var beginningDelimiter = Regex.Escape(configuration.SectionBeginningDelimiter.ToString());
            var endDelimiter = Regex.Escape(configuration.SectionEndDelimiter.ToString());
            var sectionNameRegex = configuration.SectionNameRegex.ToString();

            return new Regex($"^{whitespace}{beginningDelimiter}{sectionNameRegex}{endDelimiter}{whitespace}$");
        }

        private readonly struct AllowDuplicateKeysTransformation : IDuplicatePropertyBehaviorTransformation<bool>
        {
            public bool Fail()
                => false;

            public bool Ignore()
                => true;

            public bool Override()
                => true;
        }

        private readonly struct AllowDuplicateSectionsTransformation : IDuplicateSectionBehaviorTransformation<bool>
        {
            public bool Fail()
                => false;

            public bool Merge()
                => true;
        }

        private readonly struct CaseSensitivityTransformation : ICaseSensitivityTransformation<bool>
        {
            public bool CaseSensitive()
                => false;

            public bool CaseInsensitive()
                => true;
        }

        private readonly struct OverrideDuplicateKeysTransformation : IDuplicatePropertyBehaviorTransformation<bool>
        {
            public bool Fail()
                => false;

            public bool Ignore()
                => false;

            public bool Override()
                => true;
        }

        private readonly struct InvalidLineBehaviorTransformation : IInvalidLineBehaviorTransformation<bool>
        {
            public bool Fail()
                => false;

            public bool Ignore()
                => true;
        }

        public void SaveTo(TextWriter textWriter)
        {
            try
            {
                textWriter.Write(this.iniData.ToString());
            }
            catch (Exception exception)
            {
                throw PersistenceException.SavingFailed(exception);
            }
        }

        public IEnumerable<SectionName> EnumerateSections()
            => this.iniData.Sections.Select(section => SectionName.Create(section.SectionName));

        public IEnumerable<PropertyKey> EnumerateProperties(SectionName sectionName, PropertyEnumerationMode mode)
            => this.SelectSection(sectionName, mode.Transform(new FallbackSectionProvider(sectionName)))
                   .EnumerateProperties();

        public Comment ReadComment(SectionName sectionName, CommentReadMode mode)
            => this.SelectSection(sectionName, mode.Transform(new FallbackSectionProvider(sectionName)))
                   .ReadComment();

        public Comment ReadComment(SectionName sectionName, PropertyKey propertyKey, CommentReadMode mode)
            => this.SelectSection(sectionName, mode.Transform(new FallbackSectionProvider(sectionName)))
                   .SelectProperty(propertyKey, mode.Transform(new FallbackPropertyProvider(propertyKey)))
                   .ReadComment();

        public PropertyValue ReadProperty(SectionName sectionName, PropertyKey propertyKey, PropertyReadMode mode)
            => this.SelectSection(sectionName, mode.Transform(new FallbackSectionProvider(sectionName)))
                   .SelectProperty(propertyKey, mode.Transform(new FallbackPropertyProvider(propertyKey)))
                   .ReadValue();

        public void WriteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue, PropertyWriteMode mode)
            => mode.Transform(new WritePropertySelector(this, sectionName, propertyKey))
                   .WriteValue(propertyValue);

        public void DeleteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyDeletionMode mode)
            => this.SelectSection(sectionName, mode.Transform(new FallbackSectionProvider(sectionName)))
                   .SelectProperty(propertyKey, mode.Transform(new FallbackPropertyProvider(propertyKey)))
                   .Delete();

        public void DeleteSection(SectionName sectionName, SectionDeletionMode mode)
            => this.SelectSection(sectionName, mode.Transform(new FallbackSectionProvider(sectionName)))
                   .Delete();

        private IniParserAdapter CreateSection(SectionName sectionName)
        {
            this.iniData.Sections.AddSection(sectionName.ToString());

            return this;
        }

        private ISection SelectSection(SectionName sectionName, ISection fallbackSection)
        {
            var sectionData = this.iniData.Sections.GetSectionData(sectionName.ToString());

            return sectionData is null ? fallbackSection : new ExistentSection(this.iniData, sectionName, sectionData);
        }

        private interface ISection
        {
            void Delete();

            Comment ReadComment();

            ISection CreateProperty(PropertyKey propertyKey);

            IEnumerable<PropertyKey> EnumerateProperties();

            IProperty SelectProperty(PropertyKey propertyKey, IProperty fallbackProperty);
        }

        private sealed class NullSection : ISection
        {
            private readonly Comment fallbackComment;

            public NullSection()
                : this(Comment.None)
            {
            }

            public NullSection(Comment fallbackComment)
            {
                this.fallbackComment = fallbackComment;
            }

            public void Delete()
            {
            }

            public Comment ReadComment()
                => this.fallbackComment;

            [ExcludeFromCodeCoverage]
            public ISection CreateProperty(PropertyKey propertyKey)
                => this;

            public IEnumerable<PropertyKey> EnumerateProperties()
                => Enumerable.Empty<PropertyKey>();

            public IProperty SelectProperty(PropertyKey propertyKey, IProperty fallbackProperty)
                => fallbackProperty;
        }

        private sealed class NonexistentSection : ISection
        {
            private readonly SectionName sectionName;

            public NonexistentSection(SectionName sectionName)
            {
                this.sectionName = sectionName;
            }

            public void Delete()
                => throw new SectionNotFoundException(this.sectionName);

            public Comment ReadComment()
                => throw new SectionNotFoundException(this.sectionName);

            [ExcludeFromCodeCoverage]
            public ISection CreateProperty(PropertyKey propertyKey)
                => throw new SectionNotFoundException(this.sectionName);

            public IEnumerable<PropertyKey> EnumerateProperties()
                => throw new SectionNotFoundException(this.sectionName);

            public IProperty SelectProperty(PropertyKey propertyKey, IProperty fallbackProperty)
                => throw new SectionNotFoundException(this.sectionName);
        }

        private sealed class ExistentSection : ISection
        {
            private readonly IniData owner;

            private readonly SectionName sectionName;

            private readonly SectionData sectionData;

            public ExistentSection(IniData owner, SectionName sectionName, SectionData sectionData)
            {
                this.owner = owner;
                this.sectionName = sectionName;
                this.sectionData = sectionData;
            }

            private KeyDataCollection KeyDataCollection
                => this.sectionData.Keys;

            public void Delete()
                => this.owner.Sections.RemoveSection(this.sectionName.ToString());

            public Comment ReadComment()
                => Comment.Create(this.sectionData.Comments);

            public ISection CreateProperty(PropertyKey propertyKey)
            {
                this.KeyDataCollection.AddKey(propertyKey.ToString());

                return this;
            }

            public IEnumerable<PropertyKey> EnumerateProperties()
                => this.KeyDataCollection.Select(property => PropertyKey.Create(property.KeyName));

            public IProperty SelectProperty(PropertyKey propertyKey, IProperty fallbackProperty)
            {
                var keyData = this.KeyDataCollection.GetKeyData(propertyKey.ToString());

                return keyData is null ? fallbackProperty : new ExistentProperty(this.KeyDataCollection, propertyKey, keyData);
            }
        }

        private interface IProperty
        {
            Comment ReadComment();

            PropertyValue ReadValue();

            void WriteValue(PropertyValue value);

            void Delete();
        }

        private sealed class NullProperty : IProperty
        {
            private readonly Comment comment;

            private readonly PropertyValue propertyValue;

            private NullProperty(Comment comment, PropertyValue propertyValue)
            {
                this.comment = comment;
                this.propertyValue = propertyValue;
            }

            public NullProperty(PropertyValue propertyValue)
                : this(Comment.None, propertyValue)
            {
            }

            public NullProperty(Comment comment)
                : this(comment, PropertyValue.None)
            {
            }

            public Comment ReadComment()
                => this.comment;

            public PropertyValue ReadValue()
                => this.propertyValue;

            [ExcludeFromCodeCoverage]
            public void WriteValue(PropertyValue value)
            {
            }

            public void Delete()
            {
            }
        }

        private sealed class NonexistentProperty : IProperty
        {
            private readonly PropertyKey propertyKey;

            public NonexistentProperty(PropertyKey propertyKey)
            {
                this.propertyKey = propertyKey;
            }

            public Comment ReadComment()
                => throw new PropertyNotFoundException(this.propertyKey);

            public PropertyValue ReadValue()
                => throw new PropertyNotFoundException(this.propertyKey);

            public void WriteValue(PropertyValue value)
                => throw new PropertyNotFoundException(this.propertyKey);

            public void Delete()
                => throw new PropertyNotFoundException(this.propertyKey);
        }

        private sealed class ExistentProperty : IProperty
        {
            private readonly KeyDataCollection owner;

            private readonly PropertyKey propertyKey;

            private readonly KeyData keyData;

            public ExistentProperty(KeyDataCollection owner, PropertyKey propertyKey, KeyData keyData)
            {
                this.owner = owner;
                this.propertyKey = propertyKey;
                this.keyData = keyData;
            }

            public Comment ReadComment()
                => Comment.Create(this.keyData.Comments);

            public PropertyValue ReadValue()
                => this.keyData.Value;

            public void WriteValue(PropertyValue value)
                => this.keyData.Value = value.ToString();

            public void Delete()
                => this.owner.RemoveKey(this.propertyKey.ToString());
        }

        private sealed class FallbackSectionProvider : ISectionDeletionModeTransformation<ISection>, IPropertyEnumerationModeTransformation<ISection>, IPropertyReadModeTransformation<ISection>, IPropertyDeletionModeTransformation<ISection>, ICommentReadModeTransformation<ISection>
        {
            private readonly SectionName sectionName;

            public FallbackSectionProvider(SectionName sectionName)
            {
                this.sectionName = sectionName;
            }

            public ISection Fail()
                => new NonexistentSection(this.sectionName);

            public ISection Fallback()
                => new NullSection();

            public ISection Fallback(PropertyValue fallbackPropertyValue)
                => new NullSection();

            public ISection Fallback(Comment fallbackComment)
                => new NullSection(fallbackComment);

            public ISection Ignore()
                => new NullSection();
        }

        private sealed class FallbackPropertyProvider : IPropertyReadModeTransformation<IProperty>, IPropertyDeletionModeTransformation<IProperty>, ICommentReadModeTransformation<IProperty>
        {
            private readonly PropertyKey propertyKey;

            public FallbackPropertyProvider(PropertyKey propertyKey)
            {
                this.propertyKey = propertyKey;
            }

            public IProperty Fail()
                => new NonexistentProperty(this.propertyKey);

            public IProperty Fallback(Comment fallbackComment)
                => new NullProperty(fallbackComment);

            public IProperty Fallback(PropertyValue fallbackPropertyValue)
                => new NullProperty(fallbackPropertyValue);

            public IProperty Ignore()
                => new NullProperty(PropertyValue.None);
        }

        private sealed class WritePropertySelector : IPropertyWriteModeTransformation<IProperty>
        {
            private readonly IniParserAdapter owner;

            private readonly SectionName sectionName;

            private readonly PropertyKey propertyKey;

            public WritePropertySelector(IniParserAdapter owner, SectionName sectionName, PropertyKey propertyKey)
            {
                this.owner = owner;
                this.sectionName = sectionName;
                this.propertyKey = propertyKey;
            }

            public IProperty Create()
                => this.owner
                       .CreateSection(this.sectionName)
                       .SelectSection(this.sectionName, new NonexistentSection(this.sectionName))
                       .CreateProperty(this.propertyKey)
                       .SelectProperty(this.propertyKey, new NonexistentProperty(this.propertyKey));

            public IProperty Update()
                => this.owner
                       .SelectSection(this.sectionName, new NonexistentSection(this.sectionName))
                       .SelectProperty(this.propertyKey, new NonexistentProperty(this.propertyKey));
        }
    }
}
