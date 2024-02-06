namespace Delizious.Ini
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
        private readonly IniData iniData;

        private IniDocument(IniData iniData)
        {
            this.iniData = iniData;
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

            try
            {
                var parser = new IniDataParser(MakeIniParserConfiguration());
                var iniData = parser.Parse(textReader.ReadToEnd());

                return new IniDocument(iniData);
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

        /// <summary>
        /// Provides the names of all sections contained in the current <see cref="IniDocument"/>.
        /// </summary>
        /// <returns>
        /// The names of all sections contained in the current <see cref="IniDocument"/>
        /// </returns>
        public IEnumerable<SectionName> SectionNames()
            => this.iniData.Sections.Select(section => SectionName.Create(section.SectionName));
    }
}
