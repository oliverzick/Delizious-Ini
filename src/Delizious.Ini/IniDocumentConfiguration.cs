namespace Delizious.Ini
{
    using System;

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
            this.PropertyDeletionMode = other.PropertyDeletionMode;
            this.SectionDeletionMode = other.SectionDeletionMode;
        }

        /// <summary>
        /// <para>
        /// The default configuration of an <see cref="IniDocument"/>.
        /// </para>
        /// <para>
        /// Specifies loose modes according to the given overview:
        /// </para>
        /// <list type="table">
        /// <item>
        /// <term><see cref="PropertyEnumerationMode"/></term>
        /// <description><see cref="Ini.PropertyEnumerationMode.Fallback"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="PropertyReadMode"/></term>
        /// <description><see cref="Ini.PropertyReadMode.Fallback"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="PropertyWriteMode"/></term>
        /// <description><see cref="Ini.PropertyWriteMode.Create"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="PropertyDeletionMode"/></term>
        /// <description><see cref="Ini.PropertyDeletionMode.Ignore"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="SectionDeletionMode"/></term>
        /// <description><see cref="Ini.SectionDeletionMode.Ignore"/></description>
        /// </item>
        /// </list>
        /// </summary>
        public static IniDocumentConfiguration Default
            => new IniDocumentConfiguration();

        /// <summary>
        /// <para>
        /// The case sensitivity that specifies how to treat section names and property keys.
        /// </para>
        /// <para>
        /// Defaults to <see cref="CaseSensitivity.CaseInsensitive"/>.
        /// </para>
        /// </summary>
        public CaseSensitivity CaseSensitivity { get; } = CaseSensitivity.CaseInsensitive;

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines the case sensitivity.
        /// </summary>
        /// <param name="caseSensitivity">
        /// The case sensitivity that specifies how to treat section names and property keys.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="caseSensitivity"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="caseSensitivity"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithCaseSensitivity(CaseSensitivity caseSensitivity)
            => new IniDocumentConfiguration(this, caseSensitivity ?? throw new ArgumentNullException(nameof(caseSensitivity)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, CaseSensitivity caseSensitivity)
            : this(other)
        {
            this.CaseSensitivity = caseSensitivity;
        }

        /// <summary>
        /// The mode that specifies how to enumerate properties.
        /// </summary>
        public PropertyEnumerationMode PropertyEnumerationMode { get; } = PropertyEnumerationMode.Fallback;

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines the mode that specifies how to enumerate properties.
        /// </summary>
        /// <param name="propertyEnumerationMode">
        /// The mode that specifies how to enumerate properties.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="propertyEnumerationMode"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyEnumerationMode"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithPropertyEnumerationMode(PropertyEnumerationMode propertyEnumerationMode)
            => new IniDocumentConfiguration(this, propertyEnumerationMode ?? throw new ArgumentNullException(nameof(propertyEnumerationMode)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, PropertyEnumerationMode propertyEnumerationMode)
            : this(other)
        {
            this.PropertyEnumerationMode = propertyEnumerationMode;
        }

        /// <summary>
        /// The mode that specifies how to read a property.
        /// </summary>
        public PropertyReadMode PropertyReadMode { get; } = PropertyReadMode.Fallback;

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines the mode that specifies how to read a property.
        /// </summary>
        /// <param name="propertyReadMode">
        /// The mode that specifies how to read a property.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="propertyReadMode"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyReadMode"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithPropertyReadMode(PropertyReadMode propertyReadMode)
            => new IniDocumentConfiguration(this, propertyReadMode ?? throw new ArgumentNullException(nameof(propertyReadMode)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, PropertyReadMode propertyReadMode)
            : this(other)
        {
            this.PropertyReadMode = propertyReadMode;
        }

        /// <summary>
        /// The mode that specifies how to write a property.
        /// </summary>
        public PropertyWriteMode PropertyWriteMode { get; } = PropertyWriteMode.Create;

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines the mode that specifies how to write a property.
        /// </summary>
        /// <param name="propertyWriteMode">
        /// The mode that specifies how to write a property.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="propertyWriteMode"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyWriteMode"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithPropertyWriteMode(PropertyWriteMode propertyWriteMode)
            => new IniDocumentConfiguration(this, propertyWriteMode ?? throw new ArgumentNullException(nameof(propertyWriteMode)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, PropertyWriteMode propertyWriteMode)
            : this(other)
        {
            this.PropertyWriteMode = propertyWriteMode;
        }

        /// <summary>
        /// The mode that specifies how to delete a property.
        /// </summary>
        public PropertyDeletionMode PropertyDeletionMode { get; } = PropertyDeletionMode.Ignore;

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines the mode that specifies how to delete a property.
        /// </summary>
        /// <param name="propertyDeletionMode">
        /// The mode that specifies how to delete a property.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="propertyDeletionMode"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyDeletionMode"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithPropertyDeletionMode(PropertyDeletionMode propertyDeletionMode)
            => new IniDocumentConfiguration(this, propertyDeletionMode ?? throw new ArgumentNullException(nameof(propertyDeletionMode)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, PropertyDeletionMode propertyDeletionMode)
            : this(other)
        {
            this.PropertyDeletionMode = propertyDeletionMode;
        }

        /// <summary>
        /// The mode that specifies how to delete a section.
        /// </summary>
        public SectionDeletionMode SectionDeletionMode { get; } = SectionDeletionMode.Ignore;

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines the mode that specifies how to delete a section.
        /// </summary>
        /// <param name="sectionDeletionMode">
        /// The mode that specifies how to delete a section.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="sectionDeletionMode"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionDeletionMode"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithSectionDeletionMode(SectionDeletionMode sectionDeletionMode)
            => new IniDocumentConfiguration(this, sectionDeletionMode ?? throw new ArgumentNullException(nameof(sectionDeletionMode)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, SectionDeletionMode sectionDeletionMode)
            : this(other)
        {
            this.SectionDeletionMode = sectionDeletionMode;
        }
    }
}
