namespace Delizious.Ini
{
    using System.Collections.Generic;
    using System.IO;

    internal interface IIniDocument
    {
        IEnumerable<SectionName> EnumerateSections();

        IEnumerable<PropertyKey> EnumerateProperties(SectionName sectionName, PropertyEnumerationMode mode);

        PropertyValue ReadProperty(SectionName sectionName, PropertyKey propertyKey, PropertyReadMode mode);

        void WriteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue, PropertyWriteMode mode);

        void DeleteSection(SectionName sectionName, SectionDeletionMode mode);

        void SaveTo(TextWriter textWriter);
    }
}
