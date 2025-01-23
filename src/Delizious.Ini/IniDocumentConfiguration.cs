﻿namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Represents the configuration of an <see cref="IniDocument"/>.
    /// </summary>
    public sealed class IniDocumentConfiguration : IEquatable<IniDocumentConfiguration>
    {
        // NOTE: Parameterless constructor called by copy constructor causes equality tests to fail
        //       because state of properties is lost when instances are used in dynamic data method along with data test methods.

        private IniDocumentConfiguration(CaseSensitivity caseSensitivity,
                                         SectionBeginningDelimiter sectionBeginningDelimiter,
                                         SectionEndDelimiter sectionEndDelimiter,
                                         DuplicatePropertyBehavior duplicatePropertyBehavior,
                                         DuplicateSectionBehavior duplicateSectionBehavior,
                                         InvalidLineBehavior invalidLineBehavior,
                                         PropertyAssignmentSeparator propertyAssignmentSeparator,
                                         PropertyAssignmentSpacer propertyAssignmentSpacer,
                                         PropertyEnumerationMode propertyEnumerationMode,
                                         PropertyReadMode propertyReadMode,
                                         PropertyWriteMode propertyWriteMode,
                                         PropertyDeletionMode propertyDeletionMode,
                                         SectionDeletionMode sectionDeletionMode)
        {
            this.CaseSensitivity = caseSensitivity;
            this.SectionBeginningDelimiter = sectionBeginningDelimiter;
            this.SectionEndDelimiter = sectionEndDelimiter;
            this.DuplicatePropertyBehavior = duplicatePropertyBehavior;
            this.DuplicateSectionBehavior = duplicateSectionBehavior;
            this.InvalidLineBehavior = invalidLineBehavior;
            this.PropertyAssignmentSeparator = propertyAssignmentSeparator;
            this.PropertyAssignmentSpacer = propertyAssignmentSpacer;
            this.PropertyEnumerationMode = propertyEnumerationMode;
            this.PropertyReadMode = propertyReadMode;
            this.PropertyWriteMode = propertyWriteMode;
            this.PropertyDeletionMode = propertyDeletionMode;
            this.SectionDeletionMode = sectionDeletionMode;
        }

        private IniDocumentConfiguration(IniDocumentConfiguration other)
            : this(other.CaseSensitivity,
                   other.SectionBeginningDelimiter,
                   other.SectionEndDelimiter,
                   other.DuplicatePropertyBehavior,
                   other.DuplicateSectionBehavior,
                   other.InvalidLineBehavior,
                   other.PropertyAssignmentSeparator,
                   other.PropertyAssignmentSpacer,
                   other.PropertyEnumerationMode,
                   other.PropertyReadMode,
                   other.PropertyWriteMode,
                   other.PropertyDeletionMode,
                   other.SectionDeletionMode)
        {
        }

        /// <summary>
        /// <para>
        /// The default configuration of an <see cref="IniDocument"/>.
        /// </para>
        /// <para>
        /// Represents the <see cref="Loose"/> configuration.
        /// </para>
        /// </summary>
        public static IniDocumentConfiguration Default
            => Loose;

        /// <summary>
        /// <para>
        /// The loose configuration of an <see cref="IniDocument"/>.
        /// </para>
        /// <para>
        /// It contains the following settings:
        /// </para>
        /// <list type="table">
        /// <item>
        /// <term><see cref="CaseSensitivity"/></term>
        /// <description><see cref="Ini.CaseSensitivity.CaseInsensitive"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="SectionBeginningDelimiter"/></term>
        /// <description><see cref="Ini.SectionBeginningDelimiter.Default"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="SectionEndDelimiter"/></term>
        /// <description><see cref="Ini.SectionEndDelimiter.Default"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="DuplicatePropertyBehavior"/></term>
        /// <description><see cref="Ini.DuplicatePropertyBehavior.Ignore"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="DuplicateSectionBehavior"/></term>
        /// <description><see cref="Ini.DuplicateSectionBehavior.Merge"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="InvalidLineBehavior"/></term>
        /// <description><see cref="InvalidLineBehavior.Ignore"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="PropertyAssignmentSeparator"/></term>
        /// <description><see cref="Ini.PropertyAssignmentSeparator.Default"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="PropertyAssignmentSpacer"/></term>
        /// <description><see cref="Ini.PropertyAssignmentSpacer.None"/></description>
        /// </item>
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
        public static IniDocumentConfiguration Loose
            => new IniDocumentConfiguration(CaseSensitivity.CaseInsensitive,
                                            SectionBeginningDelimiter.Default,
                                            SectionEndDelimiter.Default,
                                            DuplicatePropertyBehavior.Ignore,
                                            DuplicateSectionBehavior.Merge,
                                            InvalidLineBehavior.Ignore,
                                            PropertyAssignmentSeparator.Default,
                                            PropertyAssignmentSpacer.None,
                                            PropertyEnumerationMode.Fallback,
                                            PropertyReadMode.Fallback,
                                            PropertyWriteMode.Create,
                                            PropertyDeletionMode.Ignore,
                                            SectionDeletionMode.Ignore);

        /// <summary>
        /// <para>
        /// The strict configuration of an <see cref="IniDocument"/>.
        /// </para>
        /// <para>
        /// It contains the following settings:
        /// </para>
        /// <list type="table">
        /// <item>
        /// <term><see cref="CaseSensitivity"/></term>
        /// <description><see cref="Ini.CaseSensitivity.CaseInsensitive"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="SectionBeginningDelimiter"/></term>
        /// <description><see cref="Ini.SectionBeginningDelimiter.Default"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="SectionEndDelimiter"/></term>
        /// <description><see cref="Ini.SectionEndDelimiter.Default"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="DuplicatePropertyBehavior"/></term>
        /// <description><see cref="Ini.DuplicatePropertyBehavior.Fail"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="DuplicateSectionBehavior"/></term>
        /// <description><see cref="Ini.DuplicateSectionBehavior.Fail"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="InvalidLineBehavior"/></term>
        /// <description><see cref="InvalidLineBehavior.Fail"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="PropertyAssignmentSeparator"/></term>
        /// <description><see cref="Ini.PropertyAssignmentSeparator.Default"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="PropertyAssignmentSpacer"/></term>
        /// <description><see cref="Ini.PropertyAssignmentSpacer.None"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="PropertyEnumerationMode"/></term>
        /// <description><see cref="Ini.PropertyEnumerationMode.Fail"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="PropertyReadMode"/></term>
        /// <description><see cref="Ini.PropertyReadMode.Fail"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="PropertyWriteMode"/></term>
        /// <description><see cref="Ini.PropertyWriteMode.Update"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="PropertyDeletionMode"/></term>
        /// <description><see cref="Ini.PropertyDeletionMode.Fail"/></description>
        /// </item>
        /// <item>
        /// <term><see cref="SectionDeletionMode"/></term>
        /// <description><see cref="Ini.SectionDeletionMode.Fail"/></description>
        /// </item>
        /// </list>
        /// </summary>
        public static IniDocumentConfiguration Strict
            => new IniDocumentConfiguration(CaseSensitivity.CaseInsensitive,
                                            SectionBeginningDelimiter.Default,
                                            SectionEndDelimiter.Default,
                                            DuplicatePropertyBehavior.Fail,
                                            DuplicateSectionBehavior.Fail,
                                            InvalidLineBehavior.Fail,
                                            PropertyAssignmentSeparator.Default,
                                            PropertyAssignmentSpacer.None,
                                            PropertyEnumerationMode.Fail,
                                            PropertyReadMode.Fail,
                                            PropertyWriteMode.Update,
                                            PropertyDeletionMode.Fail,
                                            SectionDeletionMode.Fail);

        /// <summary>
        /// <para>
        /// The case sensitivity that specifies how to treat section names and property keys.
        /// </para>
        /// </summary>
        public CaseSensitivity CaseSensitivity { get; }

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
        /// The beginning delimiter of a section.
        /// </summary>
        public SectionBeginningDelimiter SectionBeginningDelimiter { get; }

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines
        /// the beginning delimiter of a section.
        /// </summary>
        /// <param name="sectionBeginningDelimiter">
        /// The beginning delimiter of a section.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="sectionBeginningDelimiter"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionBeginningDelimiter"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithSectionBeginningDelimiter(SectionBeginningDelimiter sectionBeginningDelimiter)
            => new IniDocumentConfiguration(this, sectionBeginningDelimiter ?? throw new ArgumentNullException(nameof(sectionBeginningDelimiter)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, SectionBeginningDelimiter sectionBeginningDelimiter)
            : this(other)
        {
            this.SectionBeginningDelimiter = sectionBeginningDelimiter;
        }

        /// <summary>
        /// The end delimiter of a section.
        /// </summary>
        public SectionEndDelimiter SectionEndDelimiter { get; }

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines
        /// the end delimiter of a section.
        /// </summary>
        /// <param name="sectionEndDelimiter">
        /// The end delimiter of a section.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="sectionEndDelimiter"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionEndDelimiter"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithSectionEndDelimiter(SectionEndDelimiter sectionEndDelimiter)
            => new IniDocumentConfiguration(this, sectionEndDelimiter ?? throw new ArgumentNullException(nameof(sectionEndDelimiter)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, SectionEndDelimiter sectionEndDelimiter)
            : this(other)
        {
            this.SectionEndDelimiter = sectionEndDelimiter;
        }

        /// <summary>
        /// The behavior that specifies how an <see cref="IniDocument"/> should behave on loading when a duplicate property occurs.
        /// </summary>
        public DuplicatePropertyBehavior DuplicatePropertyBehavior { get; }

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines
        /// the behavior that specifies how an <see cref="IniDocument"/> should behave on loading when a duplicate property occurs.
        /// </summary>
        /// <param name="duplicatePropertyBehavior">
        /// The behavior that specifies how an <see cref="IniDocument"/> should behave on loading when a duplicate property occurs.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="duplicatePropertyBehavior"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="duplicatePropertyBehavior"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithDuplicatePropertyBehavior(DuplicatePropertyBehavior duplicatePropertyBehavior)
            => new IniDocumentConfiguration(this, duplicatePropertyBehavior ?? throw new ArgumentNullException(nameof(duplicatePropertyBehavior)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, DuplicatePropertyBehavior duplicatePropertyBehavior)
            : this(other)
        {
            this.DuplicatePropertyBehavior = duplicatePropertyBehavior;
        }

        /// <summary>
        /// The behavior that specifies how an <see cref="IniDocument"/> should behave on loading when a duplicate section occurs.
        /// </summary>
        public DuplicateSectionBehavior DuplicateSectionBehavior { get; }

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines
        /// the behavior that specifies how an <see cref="IniDocument"/> should behave on loading when a duplicate section occurs.
        /// </summary>
        /// <param name="duplicateSectionBehavior">
        /// The behavior that specifies how an <see cref="IniDocument"/> should behave on loading when a duplicate section occurs.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="duplicateSectionBehavior"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="duplicateSectionBehavior"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithDuplicateSectionBehavior(DuplicateSectionBehavior duplicateSectionBehavior)
            => new IniDocumentConfiguration(this, duplicateSectionBehavior ?? throw new ArgumentNullException(nameof(duplicateSectionBehavior)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, DuplicateSectionBehavior duplicateSectionBehavior)
            : this(other)
        {
            this.DuplicateSectionBehavior = duplicateSectionBehavior;
        }

        /// <summary>
        /// The behavior that specifies how an <see cref="IniDocument"/> should behave on loading when a line is invalid and cannot be parsed.
        /// </summary>
        public InvalidLineBehavior InvalidLineBehavior { get; }

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines
        /// the behavior that specifies how an <see cref="IniDocument"/> should behave on loading when a line is invalid and cannot be parsed.
        /// </summary>
        /// <param name="invalidLineBehavior">
        /// The behavior that specifies how an <see cref="IniDocument"/> should behave on loading when a line is invalid and cannot be parsed.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="invalidLineBehavior"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="invalidLineBehavior"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithInvalidLineBehavior(InvalidLineBehavior invalidLineBehavior)
            => new IniDocumentConfiguration(this, invalidLineBehavior ?? throw new ArgumentNullException(nameof(invalidLineBehavior)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, InvalidLineBehavior invalidLineBehavior)
            : this(other)
        {
            this.InvalidLineBehavior = invalidLineBehavior;
        }

        /// <summary>
        /// The assignment separator of a property.
        /// </summary>
        public PropertyAssignmentSeparator PropertyAssignmentSeparator { get; }

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines
        /// the assignment separator of a property.
        /// </summary>
        /// <param name="propertyAssignmentSeparator">
        /// The assignment separator of a property.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="propertyAssignmentSeparator"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyAssignmentSeparator"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithPropertyAssignmentSeparator(PropertyAssignmentSeparator propertyAssignmentSeparator)
            => new IniDocumentConfiguration(this, propertyAssignmentSeparator ?? throw new ArgumentNullException(nameof(propertyAssignmentSeparator)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, PropertyAssignmentSeparator propertyAssignmentSeparator)
            : this(other)
        {
            this.PropertyAssignmentSeparator = propertyAssignmentSeparator;
        }

        /// <summary>
        /// The assignment spacer that is used before and after the <see cref="PropertyAssignmentSeparator"/>
        /// when saving an <see cref="IIniDocument"/>.
        /// </summary>
        public PropertyAssignmentSpacer PropertyAssignmentSpacer { get; }

        /// <summary>
        /// Creates a copy of the current <see cref="IniDocumentConfiguration"/> instance and defines
        /// the assignment spacer that is used before and after the <see cref="PropertyAssignmentSeparator"/>
        /// when saving an <see cref="IIniDocument"/>.
        /// </summary>
        /// <param name="propertyAssignmentSpacer">
        /// The assignment spacer.
        /// </param>
        /// <returns>
        /// A new <see cref="IniDocumentConfiguration"/> instance with the given <paramref name="propertyAssignmentSpacer"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyAssignmentSpacer"/> is <c>null</c>.
        /// </exception>
        public IniDocumentConfiguration WithPropertyAssignmentSpacer(PropertyAssignmentSpacer propertyAssignmentSpacer)
            => new IniDocumentConfiguration(this, propertyAssignmentSpacer ?? throw new ArgumentNullException(nameof(propertyAssignmentSpacer)));

        private IniDocumentConfiguration(IniDocumentConfiguration other, PropertyAssignmentSpacer propertyAssignmentSpacer)
            : this(other)
        {
            this.PropertyAssignmentSpacer = propertyAssignmentSpacer;
        }

        /// <summary>
        /// The mode that specifies how to enumerate properties.
        /// </summary>
        public PropertyEnumerationMode PropertyEnumerationMode { get; }

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
        public PropertyReadMode PropertyReadMode { get; }

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
        public PropertyWriteMode PropertyWriteMode { get; }

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
        public PropertyDeletionMode PropertyDeletionMode { get; }

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
        public SectionDeletionMode SectionDeletionMode { get; }

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

        public static bool operator ==(IniDocumentConfiguration left, IniDocumentConfiguration right)
            => Equals(left, right);

        public static bool operator !=(IniDocumentConfiguration left, IniDocumentConfiguration right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(IniDocumentConfiguration other)
        {
            if (other is null)
            {
                return false;
            }

            return Equals(this.CaseSensitivity,             other.CaseSensitivity)
                && Equals(this.SectionBeginningDelimiter,   other.SectionBeginningDelimiter)
                && Equals(this.SectionEndDelimiter,         other.SectionEndDelimiter)
                && Equals(this.DuplicatePropertyBehavior,   other.DuplicatePropertyBehavior)
                && Equals(this.DuplicateSectionBehavior,    other.DuplicateSectionBehavior)
                && Equals(this.InvalidLineBehavior,         other.InvalidLineBehavior)
                && Equals(this.PropertyAssignmentSeparator, other.PropertyAssignmentSeparator)
                && Equals(this.PropertyAssignmentSpacer,    other.PropertyAssignmentSpacer)
                && Equals(this.PropertyEnumerationMode,     other.PropertyEnumerationMode)
                && Equals(this.PropertyReadMode,            other.PropertyReadMode)
                && Equals(this.PropertyWriteMode,           other.PropertyWriteMode)
                && Equals(this.PropertyDeletionMode,        other.PropertyDeletionMode)
                && Equals(this.SectionDeletionMode,         other.SectionDeletionMode);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as IniDocumentConfiguration);

        /// <inheritdoc/>
        public override int GetHashCode()
            => HashCode.Calculate(this.CaseSensitivity.GetHashCode(),
                                  this.SectionBeginningDelimiter.GetHashCode(),
                                  this.SectionEndDelimiter.GetHashCode(),
                                  this.DuplicatePropertyBehavior.GetHashCode(),
                                  this.DuplicateSectionBehavior.GetHashCode(),
                                  this.InvalidLineBehavior.GetHashCode(),
                                  this.PropertyAssignmentSeparator.GetHashCode(),
                                  this.PropertyAssignmentSpacer.GetHashCode(),
                                  this.PropertyEnumerationMode.GetHashCode(),
                                  this.PropertyReadMode.GetHashCode(),
                                  this.PropertyWriteMode.GetHashCode(),
                                  this.PropertyDeletionMode.GetHashCode(),
                                  this.SectionDeletionMode.GetHashCode());

        /// <inheritdoc/>
        public override string ToString()
            => $"{nameof(IniDocumentConfiguration)} {{ {nameof(this.CaseSensitivity)} = {this.CaseSensitivity}, {nameof(this.SectionBeginningDelimiter)} = {this.SectionBeginningDelimiter}, {nameof(this.SectionEndDelimiter)} = {this.SectionEndDelimiter}, {nameof(this.DuplicatePropertyBehavior)} = {this.DuplicatePropertyBehavior}, {nameof(this.DuplicateSectionBehavior)} = {this.DuplicateSectionBehavior}, {nameof(this.InvalidLineBehavior)} = {this.InvalidLineBehavior}, {nameof(this.PropertyAssignmentSeparator)} = {this.PropertyAssignmentSeparator}, {nameof(this.PropertyAssignmentSpacer)} = {this.PropertyAssignmentSpacer}, {nameof(this.PropertyEnumerationMode)} = {this.PropertyEnumerationMode}, {nameof(this.PropertyReadMode)} = {this.PropertyReadMode}, {nameof(this.PropertyWriteMode)} = {this.PropertyWriteMode}, {nameof(this.PropertyDeletionMode)} = {this.PropertyDeletionMode}, {nameof(this.SectionDeletionMode)} = {this.SectionDeletionMode} }}";
    }
}
