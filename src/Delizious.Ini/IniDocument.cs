namespace Delizious.Ini
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Represents an INI document.
    /// </summary>
    public sealed class IniDocument
    {
        private readonly IIniDocument iniDocument;

        private readonly IniDocumentConfiguration configuration;

        private IniDocument(IIniDocument iniDocument, IniDocumentConfiguration configuration)
        {
            this.iniDocument = iniDocument;
            this.configuration = configuration;
        }

        /// <summary>
        /// Creates a new empty INI document with the given <paramref name="configuration"/>.
        /// </summary>
        /// <param name="configuration">
        /// The configuration of the new empty INI document.
        /// </param>
        /// <returns>
        /// A new empty <see cref="IniDocument"/> instance.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configuration"/> is <c>null</c>.
        /// </exception>
        public static IniDocument CreateEmpty(IniDocumentConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return new IniDocument(IniParserAdapter.CreateEmpty(configuration), configuration);
        }

        /// <summary>
        /// Loads an INI document from the given <paramref name="textReader"/> using the given <paramref name="configuration"/>.
        /// The <paramref name="textReader"/> is only used to read the INI document from and is not kept in the returned <see cref="IniDocument"/> instance.
        /// </summary>
        /// <param name="textReader">
        /// The <see cref="TextReader"/> to read the INI document from.
        /// </param>
        /// <param name="configuration">
        /// The configuration of the INI document.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocument"/> instance that represents the INI document read from the given <paramref name="textReader"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="textReader"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="configuration"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="PersistenceException">
        /// The INI document could not be loaded from the given <paramref name="textReader"/>.
        /// Inspect <see cref="Exception.InnerException"/> for detailed error and the reason for the exception.
        /// </exception>
        public static IniDocument LoadFrom(TextReader textReader, IniDocumentConfiguration configuration)
        {
            if (textReader is null)
            {
                throw new ArgumentNullException(nameof(textReader));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            return new IniDocument(IniParserAdapter.LoadFrom(textReader, configuration), configuration);
        }

        /// <summary>
        /// Saves the INI document to the given <paramref name="textWriter"/>.
        /// </summary>
        /// <param name="textWriter">
        /// The <see cref="TextWriter"/> to save the INI document to.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="textWriter"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="PersistenceException">
        /// The INI document could not be saved to the given <paramref name="textWriter"/>.
        /// Inspect <see cref="Exception.InnerException"/> for detailed error and the reason for the exception.
        /// </exception>
        public void SaveTo(TextWriter textWriter)
        {
            if (textWriter is null)
            {
                throw new ArgumentNullException(nameof(textWriter));
            }

            this.iniDocument.SaveTo(textWriter);
        }

        /// <summary>
        /// Clones the INI document and provides a deep copy of this instance.
        /// </summary>
        /// <returns>
        /// A new <see cref="IniDocument"/> instance that is a deep copy of this instance.
        /// </returns>
        public IniDocument Clone()
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(stream))
            using (var reader = new StreamReader(stream))
            {
                this.SaveTo(writer);
                writer.Flush();

                stream.Seek(0, SeekOrigin.Begin);

                return LoadFrom(reader, this.configuration);
            }
        }

        /// <summary>
        /// Enumerates the names of all contained sections.
        /// </summary>
        /// <returns>
        /// An enumerable collection of section names for all the sections contained in the current <see cref="IniDocument"/>.
        /// </returns>
        public IEnumerable<SectionName> EnumerateSections()
            => this.iniDocument.EnumerateSections();

        /// <summary>
        /// <para>
        /// Enumerates all the properties contained in the specified section using the <see cref="IniDocumentConfiguration.PropertyEnumerationMode"/>.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.PropertyEnumerationMode"/> is <see cref="PropertyEnumerationMode.Fail"/> and the specified section does not exist,
        /// throws a <see cref="SectionNotFoundException"/>.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to enumerate the contained properties.
        /// </param>
        /// <returns>
        /// An enumerable collection of property keys for all the properties contained in the specified section of the current <see cref="IniDocument"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <see cref="IniDocumentConfiguration.PropertyEnumerationMode"/> is <see cref="PropertyEnumerationMode.Fail"/> and the specified section does not exist.
        /// </exception>
        public IEnumerable<PropertyKey> EnumerateProperties(SectionName sectionName)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            return this.EnumerateProperties(sectionName, this.configuration.PropertyEnumerationMode);
        }

        /// <summary>
        /// <para>
        /// Enumerates all the properties contained in the specified section. The mode specifies the behavior in case the section does not exist.
        /// </para>
        /// <para>
        /// When mode is <see cref="PropertyEnumerationMode.Fail"/> and the section does not exist,
        /// throws a <see cref="SectionNotFoundException"/> 
        /// </para>
        /// <para>
        /// When mode is <see cref="PropertyEnumerationMode.Fallback"/> and the section does not exist,
        /// returns an empty collection of property keys.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to enumerate the contained properties.
        /// </param>
        /// <param name="mode">
        /// The mode that specifies how to enumerate when the section does not exist.
        /// </param>
        /// <returns>
        /// An enumerable collection of property keys for all the properties contained in the specified section of the current <see cref="IniDocument"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="mode"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <paramref name="mode"/> is <see cref="PropertyEnumerationMode.Fail"/> and the specified section does not exist.
        /// </exception>
        public IEnumerable<PropertyKey> EnumerateProperties(SectionName sectionName, PropertyEnumerationMode mode)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (mode is null)
            {
                throw new ArgumentNullException(nameof(mode));
            }

            return this.iniDocument.EnumerateProperties(sectionName, mode);
        }

        /// <summary>
        /// <para>
        /// Reads the value of the property contained in the specified section using the <see cref="IniDocumentConfiguration.PropertyReadMode"/>.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.PropertyReadMode"/> is <see cref="PropertyReadMode.Fail"/> and the specified section does not exist,
        /// throws a <see cref="SectionNotFoundException"/>.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.PropertyReadMode"/> is <see cref="PropertyReadMode.Fail"/> and the specified property does not exist,
        /// throws a <see cref="PropertyNotFoundException"/>.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section containing the property.
        /// </param>
        /// <param name="propertyKey">
        /// The key of the property to read the value.
        /// </param>
        /// <returns>
        /// The value of the property.
        /// In case the property exists but has no value, an empty property value is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="propertyKey"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <see cref="IniDocumentConfiguration.PropertyReadMode"/> is <see cref="PropertyReadMode.Fail"/> and the section specified by the <paramref name="sectionName"/> does not exist.
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// <see cref="IniDocumentConfiguration.PropertyReadMode"/> is <see cref="PropertyReadMode.Fail"/> and the property specified by the <paramref name="propertyKey"/> does not exist.
        /// </exception>
        public PropertyValue ReadProperty(SectionName sectionName, PropertyKey propertyKey)
            => this.ReadProperty(sectionName, propertyKey, this.configuration.PropertyReadMode);

        /// <summary>
        /// <para>
        /// Reads the value of the property contained in the specified section.
        /// The mode specifies the behavior in case the section or property does not exist.
        /// </para>
        /// <para>
        /// When mode is <see cref="PropertyReadMode.Fail"/> and the section does not exist,
        /// throws a <see cref="SectionNotFoundException"/> 
        /// </para>
        /// <para>
        /// When mode is <see cref="PropertyReadMode.Fail"/> and the property does not exist,
        /// throws a <see cref="PropertyNotFoundException"/> 
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section containing the property.
        /// </param>
        /// <param name="propertyKey">
        /// The key of the property to read the value.
        /// </param>
        /// <param name="mode">
        /// The mode that specifies how to read the property when the section or property does not exist.
        /// </param>
        /// <returns>
        /// The value of the property.
        /// When mode is <see cref="PropertyReadMode.CustomFallback(PropertyValue)"/> and the section or property does not exist, the fallback property value given by the mode is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="propertyKey"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="mode"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <paramref name="mode"/> is <see cref="PropertyReadMode.Fail"/> and the specified section does not exist.
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// <paramref name="mode"/> is <see cref="PropertyReadMode.Fail"/> and the specified property does not exist.
        /// </exception>
        public PropertyValue ReadProperty(SectionName sectionName, PropertyKey propertyKey, PropertyReadMode mode)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (propertyKey is null)
            {
                throw new ArgumentNullException(nameof(propertyKey));
            }

            if (mode is null)
            {
                throw new ArgumentNullException(nameof(mode));
            }

            return this.iniDocument.ReadProperty(sectionName, propertyKey, mode);
        }

        /// <summary>
        /// <para>
        /// Writes the value of the property contained in the section using the <see cref="IniDocumentConfiguration.PropertyWriteMode"/>.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.PropertyWriteMode"/> is <see cref="PropertyWriteMode.Update"/> and the section does not exist,
        /// a <see cref="SectionNotFoundException"/> is thrown.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.PropertyWriteMode"/> is <see cref="PropertyWriteMode.Update"/> and the property does not exist,
        /// a <see cref="PropertyNotFoundException"/> is thrown.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section containing the property.
        /// </param>
        /// <param name="propertyKey">
        /// The key of the property to write the value.
        /// </param>
        /// <param name="propertyValue">
        /// The value of the property.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="propertyKey"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="propertyValue"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <see cref="IniDocumentConfiguration.PropertyWriteMode"/> is <see cref="PropertyWriteMode.Update"/> and the section specified by the <paramref name="sectionName"/> does not exist.
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// <see cref="IniDocumentConfiguration.PropertyWriteMode"/> is <see cref="PropertyWriteMode.Update"/> and the property specified by the <paramref name="propertyKey"/> does not exist.
        /// </exception>
        public void WriteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue)
            => this.WriteProperty(sectionName, propertyKey, propertyValue, this.configuration.PropertyWriteMode);

        /// <summary>
        /// <para>
        /// Writes the value of the property contained in the section, according to the given mode.
        /// </para>
        /// <para>
        /// When mode is <see cref="PropertyWriteMode.Update"/> and the section does not exist,
        /// a <see cref="SectionNotFoundException"/> is thrown.
        /// </para>
        /// <para>
        /// When mode is <see cref="PropertyWriteMode.Update"/> and the property does not exist,
        /// a <see cref="PropertyNotFoundException"/> is thrown.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section containing the property.
        /// </param>
        /// <param name="propertyKey">
        /// The key of the property to write the value.
        /// </param>
        /// <param name="propertyValue">
        /// The value of the property.
        /// </param>
        /// <param name="mode">
        /// The mode that specifies how to write the property.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="propertyKey"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="propertyValue"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="mode"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <paramref name="mode"/> is <see cref="PropertyWriteMode.Update"/> and the specified section does not exist.
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// <paramref name="mode"/> is <see cref="PropertyWriteMode.Update"/> and the specified property does not exist.
        /// </exception>
        public void WriteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyValue propertyValue, PropertyWriteMode mode)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (propertyKey is null)
            {
                throw new ArgumentNullException(nameof(propertyKey));
            }

            if (propertyValue is null)
            {
                throw new ArgumentNullException(nameof(propertyValue));
            }

            if (mode is null)
            {
                throw new ArgumentNullException(nameof(mode));
            }

            this.iniDocument.WriteProperty(sectionName, propertyKey, propertyValue, mode);
        }

        /// <summary>
        /// <para>
        /// Deletes the property using the <see cref="IniDocumentConfiguration.PropertyDeletionMode"/>.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.PropertyDeletionMode"/> is <see cref="PropertyDeletionMode.Fail"/> and the section does not exist, a <see cref="SectionNotFoundException"/> is thrown.
        /// If the section exists but the property to delete does not exist, a <see cref="PropertyNotFoundException"/> is thrown.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.PropertyDeletionMode"/> is <see cref="PropertyDeletionMode.Ignore"/>, it is silently ignored if the section or the property does not exist.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section containing the property.
        /// </param>
        /// <param name="propertyKey">
        /// The key of the property to delete.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="propertyKey"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <see cref="IniDocumentConfiguration.PropertyDeletionMode"/> is <see cref="PropertyDeletionMode.Fail"/> and the specified section does not exist.
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// <see cref="IniDocumentConfiguration.PropertyDeletionMode"/> is <see cref="PropertyDeletionMode.Fail"/> and the specified property does not exist.
        /// </exception>
        public void DeleteProperty(SectionName sectionName, PropertyKey propertyKey)
            => this.DeleteProperty(sectionName, propertyKey, this.configuration.PropertyDeletionMode);

        /// <summary>
        /// <para>
        /// Deletes the property according to the given mode.
        /// </para>
        /// <para>
        /// When mode is <see cref="PropertyDeletionMode.Fail"/> and the section does not exist, a <see cref="SectionNotFoundException"/> is thrown.
        /// If the section exists but the property to delete does not exist, a <see cref="PropertyNotFoundException"/> is thrown.
        /// </para>
        /// <para>
        /// When mode is <see cref="PropertyDeletionMode.Ignore"/>, it is silently ignored if the section or the property does not exist.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section containing the property.
        /// </param>
        /// <param name="propertyKey">
        /// The key of the property to delete.
        /// </param>
        /// <param name="mode">
        /// The mode that specifies how the delete to property.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="propertyKey"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="mode"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <paramref name="mode"/> is <see cref="PropertyDeletionMode.Fail"/> and the specified section does not exist.
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// <paramref name="mode"/> is <see cref="PropertyDeletionMode.Fail"/> and the specified property does not exist.
        /// </exception>
        public void DeleteProperty(SectionName sectionName, PropertyKey propertyKey, PropertyDeletionMode mode)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (propertyKey is null)
            {
                throw new ArgumentNullException(nameof(propertyKey));
            }

            if (mode is null)
            {
                throw new ArgumentNullException(nameof(mode));
            }

            this.iniDocument.DeleteProperty(sectionName, propertyKey, mode);
        }

        /// <summary>
        /// <para>
        /// Deletes the section using the <see cref="IniDocumentConfiguration.SectionDeletionMode"/>.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.SectionDeletionMode"/> is <see cref="SectionDeletionMode.Fail"/> and the section does not exist,
        /// a <see cref="SectionNotFoundException"/> is thrown.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.SectionDeletionMode"/> is <see cref="SectionDeletionMode.Ignore"/>, it is silently ignored if the section does not exist.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to delete.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <see cref="IniDocumentConfiguration.SectionDeletionMode"/> is <see cref="SectionDeletionMode.Fail"/> and the specified section does not exist.
        /// </exception>
        public void DeleteSection(SectionName sectionName)
            => this.DeleteSection(sectionName, this.configuration.SectionDeletionMode);

        /// <summary>
        /// <para>
        /// Deletes the section according to the given mode.
        /// </para>
        /// <para>
        /// When mode is <see cref="SectionDeletionMode.Fail"/> and the section does not exist,
        /// a <see cref="SectionNotFoundException"/> is thrown.
        /// </para>
        /// <para>
        /// When mode is <see cref="SectionDeletionMode.Ignore"/>, it is silently ignored if the section does not exist.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to delete.
        /// </param>
        /// <param name="mode">
        /// The mode that specifies how to delete the section.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="mode"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <paramref name="mode"/> is <see cref="SectionDeletionMode.Fail"/> and the specified section does not exist.
        /// </exception>
        public void DeleteSection(SectionName sectionName, SectionDeletionMode mode)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (mode is null)
            {
                throw new ArgumentNullException(nameof(mode));
            }

            this.iniDocument.DeleteSection(sectionName, mode);
        }

        /// <summary>
        /// <para>
        /// Reads the comment of the section using the <see cref="IniDocumentConfiguration.CommentReadMode"/>.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.CommentReadMode"/> is <see cref="CommentReadMode.Fail"/> and the section does not exist,
        /// throws a <see cref="SectionNotFoundException"/> 
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to read the comment.
        /// </param>
        /// <returns>
        /// The comment of the section.
        /// When <see cref="IniDocumentConfiguration.CommentReadMode"/> is <see cref="CommentReadMode.CustomFallback(Comment)"/> and the section does not exist,
        /// the fallback comment given by the mode is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <see cref="IniDocumentConfiguration.CommentReadMode"/> is <see cref="CommentReadMode.Fail"/> and the specified section does not exist.
        /// </exception>
        public Comment ReadComment(SectionName sectionName)
            => this.ReadComment(sectionName, this.configuration.CommentReadMode);

        /// <summary>
        /// <para>
        /// Reads the comment of the section.
        /// The mode specifies the behavior in case the section does not exist.
        /// </para>
        /// <para>
        /// When mode is <see cref="CommentReadMode.Fail"/> and the section does not exist,
        /// throws a <see cref="SectionNotFoundException"/> 
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to read the comment.
        /// </param>
        /// <param name="mode">
        /// The mode that specifies how to read the comment and behave in case the section does not exist.
        /// </param>
        /// <returns>
        /// The comment of the section.
        /// When mode is <see cref="CommentReadMode.CustomFallback(Comment)"/> and the section does not exist, the fallback comment given by the mode is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="mode"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <paramref name="mode"/> is <see cref="CommentReadMode.Fail"/> and the specified section does not exist.
        /// </exception>
        public Comment ReadComment(SectionName sectionName, CommentReadMode mode)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (mode is null)
            {
                throw new ArgumentNullException(nameof(mode));
            }

            return this.iniDocument.ReadComment(sectionName, mode);
        }

        /// <summary>
        /// <para>
        /// Reads the comment of the property using the <see cref="IniDocumentConfiguration.CommentReadMode"/>.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.CommentReadMode"/> is <see cref="CommentReadMode.Fail"/> and the section does not exist,
        /// throws a <see cref="SectionNotFoundException"/> 
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.CommentReadMode"/> is <see cref="CommentReadMode.Fail"/> and the property does not exist,
        /// throws a <see cref="PropertyNotFoundException"/>.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section containing the property.
        /// </param>
        /// <param name="propertyKey">
        /// The key of the property to read the comment.
        /// </param>
        /// <returns>
        /// The comment of the property.
        /// When <see cref="IniDocumentConfiguration.CommentReadMode"/> is <see cref="CommentReadMode.CustomFallback(Comment)"/> and the section or property does not exist,
        /// the fallback comment given by the mode is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="propertyKey"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <see cref="IniDocumentConfiguration.CommentReadMode"/> is <see cref="CommentReadMode.Fail"/> and the specified section does not exist.
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// <see cref="IniDocumentConfiguration.CommentReadMode"/> is <see cref="CommentReadMode.Fail"/> and the specified property does not exist.
        /// </exception>
        public Comment ReadComment(SectionName sectionName, PropertyKey propertyKey)
            => this.ReadComment(sectionName, propertyKey, this.configuration.CommentReadMode);

        /// <summary>
        /// <para>
        /// Reads the comment of the property.
        /// The mode specifies the behavior in case the section or property does not exist.
        /// </para>
        /// <para>
        /// When mode is <see cref="CommentReadMode.Fail"/> and the section does not exist,
        /// throws a <see cref="SectionNotFoundException"/> 
        /// </para>
        /// <para>
        /// When mode is <see cref="CommentReadMode.Fail"/> and the property does not exist,
        /// throws a <see cref="PropertyNotFoundException"/> 
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section containing the property.
        /// </param>
        /// <param name="propertyKey">
        /// The key of the property to read the comment.
        /// </param>
        /// <param name="mode">
        /// The mode that specifies how to read the comment and behave in case the section or property does not exist.
        /// </param>
        /// <returns>
        /// The comment of the property.
        /// When mode is <see cref="CommentReadMode.CustomFallback(Comment)"/> and the section or property does not exist, the fallback comment given by the mode is returned.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="propertyKey"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="mode"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <paramref name="mode"/> is <see cref="CommentReadMode.Fail"/> and the specified section does not exist.
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// <paramref name="mode"/> is <see cref="CommentReadMode.Fail"/> and the specified property does not exist.
        /// </exception>
        public Comment ReadComment(SectionName sectionName, PropertyKey propertyKey, CommentReadMode mode)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (propertyKey is null)
            {
                throw new ArgumentNullException(nameof(propertyKey));
            }

            if (mode is null)
            {
                throw new ArgumentNullException(nameof(mode));
            }

            return this.iniDocument.ReadComment(sectionName, propertyKey, mode);
        }

        /// <summary>
        /// <para>
        /// Writes the comment for the section using the <see cref="IniDocumentConfiguration.CommentWriteMode"/>.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.CommentWriteMode"/> is <see cref="CommentWriteMode.Fail"/> and the section does not exist,
        /// a <see cref="SectionNotFoundException"/> is thrown.
        /// </para>
        /// <para>
        /// When <see cref="IniDocumentConfiguration.CommentWriteMode"/> is <see cref="CommentWriteMode.Ignore"/>,
        /// it is silently ignored if the section does not exist.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to write the comment.
        /// </param>
        /// <param name="comment">
        /// The comment to write for the section.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="comment"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <see cref="IniDocumentConfiguration.CommentWriteMode"/> is <see cref="CommentWriteMode.Fail"/> and the specified section does not exist.
        /// </exception>
        public void WriteComment(SectionName sectionName, Comment comment)
            => this.WriteComment(sectionName, comment, this.configuration.CommentWriteMode);

        /// <summary>
        /// <para>
        /// Writes the comment for the section according to the given mode.
        /// </para>
        /// <para>
        /// When <paramref name="mode"/> is <see cref="CommentWriteMode.Fail"/> and the section does not exist, a <see cref="SectionNotFoundException"/> is thrown.
        /// </para>
        /// <para>
        /// When <paramref name="mode"/> is <see cref="CommentWriteMode.Ignore"/>, it is silently ignored if the section does not exist.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section to write the comment.
        /// </param>
        /// <param name="comment">
        /// The comment to write for the section.
        /// </param>
        /// <param name="mode">
        /// The mode that specifies how to write the comment.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="comment"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="mode"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <paramref name="mode"/> is <see cref="CommentWriteMode.Fail"/> and the specified section does not exist.
        /// </exception>
        public void WriteComment(SectionName sectionName, Comment comment, CommentWriteMode mode)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (comment is null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            if (mode is null)
            {
                throw new ArgumentNullException(nameof(mode));
            }

            this.iniDocument.WriteComment(sectionName, comment, mode);
        }

        /// <summary>
        /// <para>
        /// Writes the comment for the property according to the given mode.
        /// </para>
        /// <para>
        /// When mode is <see cref="CommentWriteMode.Fail"/> and the section does not exist,
        /// throws a <see cref="SectionNotFoundException"/> 
        /// </para>
        /// <para>
        /// When mode is <see cref="CommentWriteMode.Fail"/> and the property does not exist,
        /// throws a <see cref="PropertyNotFoundException"/> 
        /// </para>
        /// <para>
        /// When <paramref name="mode"/> is <see cref="CommentWriteMode.Ignore"/>,
        /// it is silently ignored if the section or property does not exist.
        /// </para>
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section containing the property.
        /// </param>
        /// <param name="propertyKey">
        /// The key of the property to write the comment.
        /// </param>
        /// <param name="comment">
        /// The comment to write for the property.
        /// </param>
        /// <param name="mode">
        /// The mode that specifies how to write the comment.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <para><paramref name="sectionName"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="propertyKey"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="comment"/> is <c>null</c>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="mode"/> is <c>null</c>.</para>
        /// </exception>
        /// <exception cref="SectionNotFoundException">
        /// <paramref name="mode"/> is <see cref="CommentWriteMode.Fail"/> and the specified section does not exist.
        /// </exception>
        /// <exception cref="PropertyNotFoundException">
        /// <paramref name="mode"/> is <see cref="CommentWriteMode.Fail"/> and the specified property does not exist.
        /// </exception>
        public void WriteComment(SectionName sectionName, PropertyKey propertyKey, Comment comment, CommentWriteMode mode)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            if (propertyKey is null)
            {
                throw new ArgumentNullException(nameof(propertyKey));
            }

            if (comment is null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            if (mode is null)
            {
                throw new ArgumentNullException(nameof(mode));
            }

            this.iniDocument.WriteComment(sectionName, propertyKey, comment, mode);
        }
    }
}
