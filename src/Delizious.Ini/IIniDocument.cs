namespace Delizious.Ini
{
    using System.Collections.Generic;
    using System.IO;

    internal interface IIniDocument : IPropertyReader
    {
        IEnumerable<SectionName> EnumerateSections();

        IEnumerable<PropertyKey> EnumerateProperties(SectionName sectionName, PropertyEnumerationMode mode);

        void WriteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue, PropertyWriteMode mode);

        void SaveTo(TextWriter textWriter);
    }
}
