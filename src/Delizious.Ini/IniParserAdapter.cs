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

        public IEnumerable<PropertyKey> EnumerateProperties(SectionName sectionName)
            => this.SelectSection(sectionName).PropertyKeys();

        public PropertyValue ReadProperty(SectionName sectionName, PropertyKey propertyKey)
            => this.SelectSection(sectionName).ReadProperty(propertyKey);

        public void UpdateProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue)
            => this.SelectSection(sectionName).WriteProperty(propertyKey, propertyValue);

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

            public void WriteProperty(PropertyKey propertyKey, PropertyValue propertyValue)
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
