namespace Delizious.Ini
{
    using System.Collections.Generic;
    using System.IO;

    internal interface IIniDocument : IPropertyEnumerator
    {
        IEnumerable<SectionName> EnumerateSections();

        PropertyValue ReadProperty(SectionName sectionName, PropertyKey propertyKey);

        void WriteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue);

        void SaveTo(TextWriter textWriter);
    }
}
