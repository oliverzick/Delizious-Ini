namespace Delizious.Ini
{
    using System.Collections.Generic;
    using System.IO;

    internal interface IIniDocument : IPropertyEnumerator, IPropertyReader
    {
        IEnumerable<SectionName> EnumerateSections();

        void UpdateProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue);

        void SaveTo(TextWriter textWriter);
    }
}
