namespace Delizious.Ini
{
    using System.Collections.Generic;
    using System.IO;

    internal interface IIniDocument : IPropertyEnumerator, IPropertyReader
    {
        IEnumerable<SectionName> EnumerateSections();

        void WriteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue, PropertyWriteMode mode);

        void SaveTo(TextWriter textWriter);
    }
}
