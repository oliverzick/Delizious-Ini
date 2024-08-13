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
        /// The mode that specifies how to write a property.
        /// </summary>
        public PropertyWriteMode PropertyWriteMode { get; } = PropertyWriteMode.Update;
    }
}
