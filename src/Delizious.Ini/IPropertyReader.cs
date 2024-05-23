namespace Delizious.Ini
{
    internal interface IPropertyReader
    {
        PropertyValue ReadProperty(SectionName sectionName, PropertyKey propertyKey, PropertyReadMode mode);
    }
}
