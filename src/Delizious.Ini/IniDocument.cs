namespace Delizious.Ini
{
    using IniParser.Model;
    using IniParser.Model.Configuration;
    using IniParser.Parser;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public sealed class IniDocument
    {
        private readonly IniData iniData;

        private IniDocument(IniData iniData)
        {
            this.iniData = iniData;
        }

        public static IniDocument LoadFrom(TextReader textReader)
        {
            if (textReader is null)
            {
                throw new ArgumentNullException(nameof(textReader));
            }

            try
            {
                var configuration = new IniParserConfiguration
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

                var parser = new IniDataParser(configuration);
                var iniData = parser.Parse(textReader.ReadToEnd());

                return new IniDocument(iniData);
            }
            catch (Exception exception)
            {
                throw new IniException(ExceptionMessages.CouldNotLoadIniDocument, exception);
            }
        }

        public IEnumerable<SectionName> SectionNames()
            => this.iniData.Sections.Select(section => SectionName.Create(section.SectionName));
    }
}
