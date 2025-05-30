﻿namespace Delizious.Ini
{
    using System.Collections.Generic;
    using System.IO;

    internal interface IIniDocument
    {
        IEnumerable<SectionName> EnumerateSections();

        IEnumerable<PropertyKey> EnumerateProperties(SectionName sectionName, PropertyEnumerationMode mode);

        Comment ReadComment(SectionName sectionName, CommentReadMode mode);

        Comment ReadComment(SectionName sectionName, PropertyKey propertyKey, CommentReadMode mode);

        PropertyValue ReadProperty(SectionName sectionName, PropertyKey propertyKey, PropertyReadMode mode);

        void WriteComment(SectionName sectionName, Comment comment, CommentWriteMode mode);

        void WriteComment(SectionName sectionName, PropertyKey propertyKey, Comment comment, CommentWriteMode mode);

        void WriteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue, PropertyWriteMode mode);

        void DeleteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyDeletionMode mode);

        void DeleteSection(SectionName sectionName, SectionDeletionMode mode);

        void SaveTo(TextWriter textWriter);
    }
}
