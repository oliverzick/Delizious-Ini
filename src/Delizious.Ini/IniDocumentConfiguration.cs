﻿namespace Delizious.Ini
{
    /// <summary>
    /// Represents the configuration of an <see cref="IniDocument"/>.
    /// </summary>
    public sealed class IniDocumentConfiguration
    {
        private IniDocumentConfiguration()
        {
        }

        private IniDocumentConfiguration(IniDocumentConfiguration other)
        {
            this.PropertyEnumerationMode = other.PropertyEnumerationMode;
            this.PropertyReadMode = other.PropertyReadMode;
            this.PropertyWriteMode = other.PropertyWriteMode;
        }

        /// <summary>
        /// The default configuration of an <see cref="IniDocument"/>.
        /// </summary>
        public static IniDocumentConfiguration Default
            => new IniDocumentConfiguration();

        /// <summary>
        /// The mode that specifies how to enumerate properties.
        /// </summary>
        public PropertyEnumerationMode PropertyEnumerationMode { get; } = PropertyEnumerationMode.Fail;

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines the mode that specifies how to enumerate properties.
        /// </summary>
        /// <param name="propertyEnumerationMode">
        /// The mode that specifies how to enumerate properties.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="propertyEnumerationMode"/>.
        /// </returns>
        public IniDocumentConfiguration WithPropertyEnumerationMode(PropertyEnumerationMode propertyEnumerationMode)
            => new IniDocumentConfiguration(this, propertyEnumerationMode);

        private IniDocumentConfiguration(IniDocumentConfiguration other, PropertyEnumerationMode propertyEnumerationMode)
            : this(other)
        {
            this.PropertyEnumerationMode = propertyEnumerationMode;
        }

        /// <summary>
        /// The mode that specifies how to read a property.
        /// </summary>
        public PropertyReadMode PropertyReadMode { get; } = PropertyReadMode.Fail;

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines the mode that specifies how to read a property.
        /// </summary>
        /// <param name="propertyReadMode">
        /// The mode that specifies how to read a property.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="propertyReadMode"/>.
        /// </returns>
        public IniDocumentConfiguration WithPropertyReadMode(PropertyReadMode propertyReadMode)
            => new IniDocumentConfiguration(this, propertyReadMode);

        private IniDocumentConfiguration(IniDocumentConfiguration other, PropertyReadMode propertyReadMode)
            : this(other)
        {
            this.PropertyReadMode = propertyReadMode;
        }

        /// <summary>
        /// The mode that specifies how to write a property.
        /// </summary>
        public PropertyWriteMode PropertyWriteMode { get; } = PropertyWriteMode.Update;

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines the mode that specifies how to write a property.
        /// </summary>
        /// <param name="propertyWriteMode">
        /// The mode that specifies how to write a property.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="propertyWriteMode"/>.
        /// </returns>
        public IniDocumentConfiguration WithPropertyWriteMode(PropertyWriteMode propertyWriteMode)
            => new IniDocumentConfiguration(this, propertyWriteMode);

        private IniDocumentConfiguration(IniDocumentConfiguration other, PropertyWriteMode propertyWriteMode)
            : this(other)
        {
            this.PropertyWriteMode = propertyWriteMode;
        }

        /// <summary>
        /// The mode that specifies how to delete a section.
        /// </summary>
        public SectionDeletionMode SectionDeletionMode { get; } = SectionDeletionMode.Fail;

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines the mode that specifies how to delete a section.
        /// </summary>
        /// <param name="sectionDeletionMode">
        /// The mode that specifies how to delete a section.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="sectionDeletionMode"/>.
        /// </returns>
        public IniDocumentConfiguration WithSectionDeletionMode(SectionDeletionMode sectionDeletionMode)
            => new IniDocumentConfiguration(this, sectionDeletionMode);

        private IniDocumentConfiguration(IniDocumentConfiguration other, SectionDeletionMode sectionDeletionMode)
            : this(other)
        {
            this.SectionDeletionMode = sectionDeletionMode;
        }
    }
}
