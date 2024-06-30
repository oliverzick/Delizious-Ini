namespace Delizious.Ini
{
    using IniParser.Model;
    using IniParser.Model.Configuration;
    using IniParser.Parser;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;

    internal sealed class IniParserAdapter : IIniDocument
    {
        private readonly IniData iniData;

        private IniParserAdapter(IniData iniData)
        {
            this.iniData = iniData;
        }

        public static IniParserAdapter LoadFrom(TextReader textReader)
        {
            try
            {
                var parser = new IniDataParser(MakeIniParserConfiguration());
                var iniData = parser.Parse(textReader.ReadToEnd());

                return new IniParserAdapter(iniData);
            }
            catch (Exception exception)
            {
                throw PersistenceException.LoadingFailed(exception);
            }
        }

        private static IniParserConfiguration MakeIniParserConfiguration()
            => new IniParserConfiguration
            {
                AllowCreateSectionsOnFly = false,
                AllowDuplicateKeys = false,
                AllowDuplicateSections = false,
                AllowKeysWithoutSection = false,
                AssigmentSpacer = string.Empty,
                CaseInsensitive = true,
                ConcatenateDuplicateKeys = false,
                SkipInvalidLines = true,
                ThrowExceptionsOnError = true
            };

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

        public PropertyValue ReadProperty(SectionName sectionName, PropertyKey propertyKey, PropertyReadMode mode)
            => this.SelectSection(sectionName, mode.Transform(new FallbackSectionProvider(sectionName)))
                   .SelectProperty(propertyKey, mode.Transform(new FallbackPropertyProvider(propertyKey)))
                   .ReadValue();

        public void WriteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue, PropertyWriteMode mode)
            => mode.Transform(new WritePropertySelector(this, sectionName, propertyKey))
                   .WriteValue(propertyValue);

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
            var keyDataCollection = this.iniData.Sections[sectionName.ToString()];

            return keyDataCollection is null ? fallbackSection : new ExistentSection(this.iniData, sectionName, keyDataCollection);
        }

        private interface ISection
        {
            void Delete();

            ISection CreateProperty(PropertyKey propertyKey);

            IEnumerable<PropertyKey> EnumerateProperties();

            IProperty SelectProperty(PropertyKey propertyKey, IProperty fallbackProperty);
        }

        private sealed class NullSection : ISection
        {
            public void Delete()
            {
            }

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

            private readonly KeyDataCollection keyDataCollection;

            public ExistentSection(IniData owner, SectionName sectionName, KeyDataCollection keyDataCollection)
            {
                this.owner = owner;
                this.sectionName = sectionName;
                this.keyDataCollection = keyDataCollection;
            }

            public void Delete()
                => this.owner.Sections.RemoveSection(this.sectionName.ToString());

            public ISection CreateProperty(PropertyKey propertyKey)
            {
                this.keyDataCollection.AddKey(propertyKey.ToString());

                return this;
            }

            public IEnumerable<PropertyKey> EnumerateProperties()
                => this.keyDataCollection.Select(property => PropertyKey.Create(property.KeyName));

            public IProperty SelectProperty(PropertyKey propertyKey, IProperty fallbackProperty)
            {
                var keyData = this.keyDataCollection.GetKeyData(propertyKey.ToString());

                return keyData is null ? fallbackProperty : new ExistentProperty(keyData);
            }
        }

        private interface IProperty
        {
            PropertyValue ReadValue();

            void WriteValue(PropertyValue value);
        }

        private sealed class NullProperty : IProperty
        {
            private readonly PropertyValue propertyValue;

            public NullProperty(PropertyValue propertyValue)
            {
                this.propertyValue = propertyValue;
            }

            public PropertyValue ReadValue()
                => this.propertyValue;

            [ExcludeFromCodeCoverage]
            public void WriteValue(PropertyValue value)
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

            public PropertyValue ReadValue()
                => throw new PropertyNotFoundException(this.propertyKey);

            public void WriteValue(PropertyValue value)
                => throw new PropertyNotFoundException(this.propertyKey);
        }

        private sealed class ExistentProperty : IProperty
        {
            private readonly KeyData keyData;

            public ExistentProperty(KeyData keyData)
            {
                this.keyData = keyData;
            }

            public PropertyValue ReadValue()
                => this.keyData.Value;

            public void WriteValue(PropertyValue value)
                => this.keyData.Value = value.ToString();
        }

        private sealed class FallbackSectionProvider : ISectionDeletionModeTransformation<ISection>, IPropertyEnumerationModeTransformation<ISection>, IPropertyReadModeTransformation<ISection>
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

            public ISection Ignore()
                => new NullSection();
        }

        private sealed class FallbackPropertyProvider : IPropertyReadModeTransformation<IProperty>
        {
            private readonly PropertyKey propertyKey;

            public FallbackPropertyProvider(PropertyKey propertyKey)
            {
                this.propertyKey = propertyKey;
            }

            public IProperty Fail()
                => new NonexistentProperty(propertyKey);

            public IProperty Fallback(PropertyValue fallbackPropertyValue)
                => new NullProperty(fallbackPropertyValue);
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
