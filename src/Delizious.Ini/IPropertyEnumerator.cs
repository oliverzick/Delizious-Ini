namespace Delizious.Ini
{
    using System.Collections.Generic;

    internal interface IPropertyEnumerator
    {
        IEnumerable<PropertyKey> EnumerateProperties(SectionName sectionName);
    }
}
