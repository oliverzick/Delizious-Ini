﻿namespace Delizious.Ini
{
    using IniParser.Model;
    using IniParser.Model.Configuration;
    using IniParser.Parser;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Represents an INI document.
    /// </summary>
    public sealed class IniDocument
    {
        private readonly Content content;

        private IniDocument(Content content)
        {
            this.content = content;
        }

        /// <summary>
        /// Loads an INI document by reading the INI contents from the given <paramref name="textReader"/>. 
        /// The <paramref name="textReader"/> is only used to read the INI contents from and not kept in the returned <see cref="IniDocument"/> instance.
        /// </summary>
        /// <param name="textReader">
        /// The <see cref="TextReader"/> to read the INI contents from.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocument"/> instance that contains the INI contents read from the given <paramref name="textReader"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="textReader"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="IniException">
        /// The INI document could not be loaded from the given <paramref name="textReader"/>.
        /// </exception>
        public static IniDocument LoadFrom(TextReader textReader)
        {
            if (textReader is null)
            {
                throw new ArgumentNullException(nameof(textReader));
            }

            return new IniDocument(Content.LoadFrom(textReader));
        }

        /// <summary>
        /// Provides the names of all sections contained in the current <see cref="IniDocument"/>.
        /// </summary>
        /// <returns>
        /// The names of all sections contained in the current <see cref="IniDocument"/>
        /// </returns>
        public IEnumerable<SectionName> SectionNames()
            => this.content.SectionNames();

        /// <summary>
        /// Provides the keys of all properties contained in a section given by the <paramref name="sectionName"/>.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section.
        /// </param>
        /// <returns>
        /// The keys of all properties contained in the specified section.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// The section specified by the <paramref name="sectionName"/> does not exist.
        /// </exception>
        public IEnumerable<PropertyKey> PropertyKeys(SectionName sectionName)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            return this.content.FindSection(sectionName).PropertyKeys();
        }

        private sealed class Content
        {
            private readonly IniData iniData;

            private Content(IniData iniData)
            {
                this.iniData = iniData;
            }

            public static Content LoadFrom(TextReader textReader)
            {
                try
                {
                    var parser = new IniDataParser(MakeIniParserConfiguration());
                    var iniData = parser.Parse(textReader.ReadToEnd());

                    return new Content(iniData);
                }
                catch (Exception exception)
                {
                    throw new IniException(ExceptionMessages.CouldNotLoadIniDocument, exception);
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

            public IEnumerable<SectionName> SectionNames()
                => this.iniData.Sections.Select(section => SectionName.Create(section.SectionName));

            public Section FindSection(SectionName sectionName)
            {
                var properties = this.iniData.Sections[sectionName.ToString()] ?? throw new SectionNotFoundException(sectionName);

                return Section.Create(properties);
            }
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
        }
    }
}
