namespace Delizious.Ini
{
    using IniParser.Model;
    using IniParser.Model.Configuration;
    using IniParser.Parser;
    using System;
    using System.Collections.Generic;
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
            => mode.Transform(new PropertyEnumeratorSelector()).EnumerateProperties(this, sectionName);

        private interface IPropertyEnumerator
        {
            IEnumerable<PropertyKey> EnumerateProperties(IniParserAdapter target, SectionName sectionName);
        }

        private sealed class PropertyEnumeratorSelector : IPropertyEnumerationModeTransformation<IPropertyEnumerator>
        {
            public IPropertyEnumerator Fail()
                => new FailPropertyEnumerator();

            private sealed class FailPropertyEnumerator : IPropertyEnumerator
            {
                public IEnumerable<PropertyKey> EnumerateProperties(IniParserAdapter target, SectionName sectionName)
                    => target.SelectSection(sectionName).PropertyKeys();
            }

            public IPropertyEnumerator Fallback()
                => new FallbackPropertyEnumerator();

            private sealed class FallbackPropertyEnumerator : IPropertyEnumerator
            {
                public IEnumerable<PropertyKey> EnumerateProperties(IniParserAdapter target, SectionName sectionName)
                {
                    try
                    {
                        return target.SelectSection(sectionName).PropertyKeys();
                    }
                    catch (SectionNotFoundException)
                    {
                        return Enumerable.Empty<PropertyKey>();
                    }
                }
            }
        }

        public PropertyValue ReadProperty(SectionName sectionName, PropertyKey propertyKey, PropertyReadMode mode)
            => mode.Transform(new PropertyReaderSelector()).ReadProperty(this, sectionName, propertyKey);

        private interface IPropertyReader
        {
            PropertyValue ReadProperty(IniParserAdapter target, SectionName sectionName, PropertyKey propertyKey);
        }

        private sealed class PropertyReaderSelector : IPropertyReadModeTransformation<IPropertyReader>
        {
            public IPropertyReader Fail()
                => new FailPropertyReader();

            private sealed class FailPropertyReader : IPropertyReader
            {
                public PropertyValue ReadProperty(IniParserAdapter target, SectionName sectionName, PropertyKey propertyKey)
                    => target.SelectSection(sectionName).ReadProperty(propertyKey);
            }

            public IPropertyReader Fallback(PropertyValue fallbackPropertyValue)
                => new FallbackPropertyReader(fallbackPropertyValue);

            private sealed class FallbackPropertyReader : IPropertyReader
            {
                private readonly PropertyValue fallbackPropertyValue;

                public FallbackPropertyReader(PropertyValue fallbackPropertyValue)
                {
                    this.fallbackPropertyValue = fallbackPropertyValue;
                }

                public PropertyValue ReadProperty(IniParserAdapter target, SectionName sectionName, PropertyKey propertyKey)
                {
                    try
                    {
                        return target.SelectSection(sectionName).ReadProperty(propertyKey);
                    }
                    catch (SectionNotFoundException)
                    {
                        return this.fallbackPropertyValue;
                    }
                    catch (PropertyNotFoundException)
                    {
                        return this.fallbackPropertyValue;
                    }
                }
            }
        }

        public void WriteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue, PropertyWriteMode mode)
            => mode.Transform(new PropertyWriterSelector()).WriteProperty(this, sectionName, propertyKey, propertyValue);

        private interface IPropertyWriter
        {
            void WriteProperty(IniParserAdapter target, SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue);
        }

        private sealed class PropertyWriterSelector : IPropertyWriteModeTransformation<IPropertyWriter>
        {
            public IPropertyWriter Create()
                => new CreatePropertyWriter();

            private sealed class CreatePropertyWriter : IPropertyWriter
            {
                public void WriteProperty(IniParserAdapter target, SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue)
                    => throw new NotImplementedException();
            }

            public IPropertyWriter Update()
                => new UpdatePropertyWriter();

            private sealed class UpdatePropertyWriter : IPropertyWriter
            {
                public void WriteProperty(IniParserAdapter target, SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue)
                    => target.SelectSection(sectionName).UpdateProperty(propertyKey, propertyValue);
            }
        }

        private Section SelectSection(SectionName sectionName)
        {
            // Indexer returns a null reference when section does not exist, so we need to throw appropriate exception here
            var properties = this.iniData.Sections[sectionName.ToString()] ?? throw new SectionNotFoundException(sectionName);

            return Section.Create(properties);
        }

        private sealed class Section
        {
            private readonly KeyDataCollection properties;

            private Section(KeyDataCollection properties)
            {
                this.properties = properties;
            }

            public static Section Create(KeyDataCollection properties)
                => new Section(properties);

            public IEnumerable<PropertyKey> PropertyKeys()
                => this.properties.Select(property => PropertyKey.Create(property.KeyName));

            public PropertyValue ReadProperty(PropertyKey propertyKey)
                => this.SelectProperty(propertyKey).ReadValue();

            public void UpdateProperty(PropertyKey propertyKey, PropertyValue propertyValue)
                => this.SelectProperty(propertyKey).UpdateValue(propertyValue);

            private Property SelectProperty(PropertyKey propertyKey)
            {
                // GetKeyData returns a null reference when property does not exist, so we need to throw appropriate exception here
                var keyData = this.properties.GetKeyData(propertyKey.ToString()) ?? throw new PropertyNotFoundException(propertyKey);

                return Property.Create(keyData);
            }

            private sealed class Property
            {
                private readonly KeyData property;

                private Property(KeyData property)
                {
                    this.property = property;
                }

                public static Property Create(KeyData keyData)
                    => new Property(keyData);

                public PropertyValue ReadValue()
                    => this.property.Value;

                public void UpdateValue(PropertyValue newPropertyValue)
                    => this.property.Value = newPropertyValue.ToString();
            }
        }
    }
}
