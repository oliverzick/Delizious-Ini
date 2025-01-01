namespace Delizious.Ini.Test;

using System;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public sealed class IniDocumentConfigurationSpec
{
    private static IniDocumentConfiguration Target => IniDocumentConfiguration.Default;

    public static IEnumerable<Setting> AllSettings =>
    [
        Setting.CaseSensitivity,
        Setting.InvalidLineBehavior,
        Setting.PropertyAssignmentSeparator,
        Setting.PropertyEnumerationMode,
        Setting.PropertyReadMode,
        Setting.PropertyWriteMode,
        Setting.PropertyDeletionMode,
        Setting.SectionDeletionMode
    ];

    [TestClass]
    public sealed class Defaults
    {
        [DataTestMethod]
        [DynamicData(nameof(Default_case_sensitivity_test_cases), DynamicDataSourceType.Method)]
        public void Default_case_sensitivity(IniDocumentConfiguration target, CaseSensitivity expected)
        {
            var actual = target.CaseSensitivity;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_case_sensitivity_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, CaseSensitivity.CaseInsensitive];
            yield return [IniDocumentConfiguration.Loose, CaseSensitivity.CaseInsensitive];
            yield return [IniDocumentConfiguration.Strict, CaseSensitivity.CaseInsensitive];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_invalid_line_behavior_test_cases), DynamicDataSourceType.Method)]
        public void Default_invalid_line_behavior(IniDocumentConfiguration target, InvalidLineBehavior expected)
        {
            var actual = target.InvalidLineBehavior;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_invalid_line_behavior_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, InvalidLineBehavior.Ignore];
            yield return [IniDocumentConfiguration.Loose, InvalidLineBehavior.Ignore];
            yield return [IniDocumentConfiguration.Strict, InvalidLineBehavior.Fail];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_property_assignment_separator_test_cases), DynamicDataSourceType.Method)]
        public void Default_property_assignment_separator(IniDocumentConfiguration target, PropertyAssignmentSeparator expected)
        {
            var actual = target.PropertyAssignmentSeparator;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_property_assignment_separator_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, PropertyAssignmentSeparator.Default];
            yield return [IniDocumentConfiguration.Loose, PropertyAssignmentSeparator.Default];
            yield return [IniDocumentConfiguration.Strict, PropertyAssignmentSeparator.Default];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_property_enumeration_mode_test_cases), DynamicDataSourceType.Method)]
        public void Default_property_enumeration_mode(IniDocumentConfiguration target, PropertyEnumerationMode expected)
        {
            var actual = target.PropertyEnumerationMode;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_property_enumeration_mode_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, PropertyEnumerationMode.Fallback];
            yield return [IniDocumentConfiguration.Loose, PropertyEnumerationMode.Fallback];
            yield return [IniDocumentConfiguration.Strict, PropertyEnumerationMode.Fail];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_property_read_mode_test_cases), DynamicDataSourceType.Method)]
        public void Default_property_read_mode(IniDocumentConfiguration target, PropertyReadMode expected)
        {
            var actual = target.PropertyReadMode;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_property_read_mode_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, PropertyReadMode.Fallback];
            yield return [IniDocumentConfiguration.Loose, PropertyReadMode.Fallback];
            yield return [IniDocumentConfiguration.Strict, PropertyReadMode.Fail];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_property_write_mode_test_cases), DynamicDataSourceType.Method)]
        public void Default_property_write_mode(IniDocumentConfiguration target, PropertyWriteMode expected)
        {
            var actual = target.PropertyWriteMode;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_property_write_mode_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, PropertyWriteMode.Create];
            yield return [IniDocumentConfiguration.Loose, PropertyWriteMode.Create];
            yield return [IniDocumentConfiguration.Strict, PropertyWriteMode.Update];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_property_deletion_mode_test_cases), DynamicDataSourceType.Method)]
        public void Default_property_deletion_mode(IniDocumentConfiguration target, PropertyDeletionMode expected)
        {
            var actual = target.PropertyDeletionMode;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_property_deletion_mode_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, PropertyDeletionMode.Ignore];
            yield return [IniDocumentConfiguration.Loose, PropertyDeletionMode.Ignore];
            yield return [IniDocumentConfiguration.Strict, PropertyDeletionMode.Fail];
        }

        [DataTestMethod]
        [DynamicData(nameof(Default_section_deletion_mode_test_cases), DynamicDataSourceType.Method)]
        public void Default_section_deletion_mode(IniDocumentConfiguration target, SectionDeletionMode expected)
        {
            var actual = target.SectionDeletionMode;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Default_section_deletion_mode_test_cases()
        {
            yield return [IniDocumentConfiguration.Default, SectionDeletionMode.Ignore];
            yield return [IniDocumentConfiguration.Loose, SectionDeletionMode.Ignore];
            yield return [IniDocumentConfiguration.Strict, SectionDeletionMode.Fail];
        }
    }

    [TestClass]
    public sealed class WithCaseSensitivity
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.CaseSensitivity];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_case_sensitivity_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithCaseSensitivity(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithCaseSensitivity(CaseSensitivity.CaseSensitive);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithInvalidLineBehavior
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.InvalidLineBehavior];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_invalid_line_behavior_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithInvalidLineBehavior(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithInvalidLineBehavior(InvalidLineBehavior.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyAssignmentSeparator
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.PropertyAssignmentSeparator];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_invalid_line_behavior_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyAssignmentSeparator(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithPropertyAssignmentSeparator(':');

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyEnumerationMode
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.PropertyEnumerationMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_enumeration_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyEnumerationMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithPropertyEnumerationMode(PropertyEnumerationMode.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyReadMode
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.PropertyReadMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_read_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyReadMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithPropertyReadMode(PropertyReadMode.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyWriteMode
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.PropertyWriteMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_write_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyWriteMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithPropertyWriteMode(PropertyWriteMode.Update);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyDeletionMode
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.PropertyDeletionMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_deletion_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyDeletionMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithPropertyDeletionMode(PropertyDeletionMode.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithSectionDeletionMode
    {
        private static IEnumerable<Setting> SettingsToExclude => [Setting.SectionDeletionMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_section_deletion_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithSectionDeletionMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Setting setting)
        {
            var original = Target;

            var actual = original.WithSectionDeletionMode(SectionDeletionMode.Fail);

            setting.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllSettings.Except(SettingsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class ValueSemantics
    {
        private static IniDocumentConfiguration Null => null!;
        private static IniDocumentConfiguration Default => IniDocumentConfiguration.Default;
        private static IniDocumentConfiguration Loose => IniDocumentConfiguration.Loose;
        private static IniDocumentConfiguration Strict => IniDocumentConfiguration.Strict;

        [DataTestMethod]
        [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
        public void Provides_string_representation(IniDocumentConfiguration target)
        {
            var expected = $"{nameof(IniDocumentConfiguration)} {{ {string.Join(", ", AllSettings.Select(setting => setting.BuildString(target)))} }}";

            var actual = target.ToString();

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Provides_string_representation_test_cases()
        {
            yield return [Default];
            yield return [Loose];
            yield return [Strict];

            // ToDo: Reintroduce cases for single settings?
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
        public void Properly_implements_equality_operator(IniDocumentConfiguration left, IniDocumentConfiguration right, bool expected)
        {
            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
        public void Properly_implements_inequality_operator(IniDocumentConfiguration left, IniDocumentConfiguration right, bool inverse_expected)
        {
            var expected = !inverse_expected;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_equals_method(IniDocumentConfiguration target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
        public void Properly_implements_general_equals_method(IniDocumentConfiguration target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_get_hash_code_method(IniDocumentConfiguration target, IniDocumentConfiguration other, bool expected)
        {
            var actual = target.GetHashCode() == other.GetHashCode();

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Equality_operator_test_cases()
        {
            yield return [Null, Null, true];
            yield return [Null, Default, false];
            yield return [Null, Loose, false];
            yield return [Null, Strict, false];
        }

        public static IEnumerable<object[]> Equals_null_test_cases()
        {
            yield return [Default, Null, false];
            yield return [Loose, Null, false];
            yield return [Strict, Null, false];
        }

        public static IEnumerable<object[]> Equals_test_cases()
        {
            yield return [Default, Default, true];
            yield return [Default, Loose, true];
            yield return [Default, Strict, false];
            yield return [Default, Default.WithCaseSensitivity(CaseSensitivity.CaseSensitive), false];
            yield return [Default, Default.WithCaseSensitivity(CaseSensitivity.CaseInsensitive), true];
            yield return [Default, Default.WithPropertyDeletionMode(PropertyDeletionMode.Fail), false];
            yield return [Default, Default.WithPropertyDeletionMode(PropertyDeletionMode.Ignore), true];
            yield return [Default, Default.WithPropertyAssignmentSeparator(PropertyAssignmentSeparator.Default), true];
            yield return [Default, Default.WithPropertyAssignmentSeparator(':'), false];
            yield return [Default, Default.WithPropertyEnumerationMode(PropertyEnumerationMode.Fail), false];
            yield return [Default, Default.WithPropertyEnumerationMode(PropertyEnumerationMode.Fallback), true];
            yield return [Default, Default.WithPropertyReadMode(PropertyReadMode.Fail), false];
            yield return [Default, Default.WithPropertyReadMode(PropertyReadMode.Fallback), true];
            yield return [Default, Default.WithPropertyWriteMode(PropertyWriteMode.Create), true];
            yield return [Default, Default.WithPropertyWriteMode(PropertyWriteMode.Update), false];
            yield return [Default, Default.WithSectionDeletionMode(SectionDeletionMode.Fail), false];
            yield return [Default, Default.WithSectionDeletionMode(SectionDeletionMode.Ignore), true];

            yield return [Loose, Default, true];
            yield return [Loose, Loose, true];
            yield return [Loose, Strict, false];

            yield return [Strict, Default, false];
            yield return [Strict, Loose, false];
            yield return [Strict, Strict, true];
        }

        public static IEnumerable<object[]> General_equals_test_cases()
        {
            yield return [Default, new(), false];
            yield return [Loose, new(), false];
            yield return [Strict, new(), false];
        }
    }

    private static object[] ToTestCase(Setting setting)
        => [setting];

    public sealed record Setting
    {
        private readonly Strategy strategy;

        private Setting(Strategy strategy)
        {
            this.strategy = strategy;
        }

        public void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
            => this.strategy.AssertIsEqual(original, actual);

        public string BuildString(IniDocumentConfiguration target)
            => this.strategy.BuildString(target);

        public override string ToString()
            => this.strategy.ToString();

        private abstract record Strategy
        {
            public abstract void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual);

            public abstract string BuildString(IniDocumentConfiguration target);
        }

        public static Setting CaseSensitivity
            => new(new CaseSensitivitySetting());

        private sealed record CaseSensitivitySetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.CaseSensitivity, actual.CaseSensitivity);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.CaseSensitivity)} = {target.CaseSensitivity}";
        }

        public static Setting InvalidLineBehavior
            => new(new InvalidLineBehaviorSetting());

        private sealed record InvalidLineBehaviorSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.InvalidLineBehavior, actual.InvalidLineBehavior);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.InvalidLineBehavior)} = {target.InvalidLineBehavior}";
        }

        public static Setting PropertyAssignmentSeparator
            => new(new PropertyAssignmentSeparatorSetting());

        private sealed record PropertyAssignmentSeparatorSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyAssignmentSeparator, actual.PropertyAssignmentSeparator);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.PropertyAssignmentSeparator)} = {target.PropertyAssignmentSeparator}";
        }

        public static Setting PropertyEnumerationMode
            => new(new PropertyEnumerationModeSetting());

        private sealed record PropertyEnumerationModeSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyEnumerationMode, actual.PropertyEnumerationMode);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.PropertyEnumerationMode)} = {target.PropertyEnumerationMode}";
        }

        public static Setting PropertyReadMode
            => new(new PropertyReadModeSetting());

        private sealed record PropertyReadModeSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyReadMode, actual.PropertyReadMode);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.PropertyReadMode)} = {target.PropertyReadMode}";
        }

        public static Setting PropertyWriteMode
            => new(new PropertyWriteModeSetting());

        private sealed record PropertyWriteModeSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyWriteMode, actual.PropertyWriteMode);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.PropertyWriteMode)} = {target.PropertyWriteMode}";
        }

        public static Setting PropertyDeletionMode
            => new(new PropertyDeletionModeSetting());

        private sealed record PropertyDeletionModeSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyDeletionMode, actual.PropertyDeletionMode);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.PropertyDeletionMode)} = {target.PropertyDeletionMode}";
        }

        public static Setting SectionDeletionMode
            => new(new SectionDeletionModeSetting());

        private sealed record SectionDeletionModeSetting : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.SectionDeletionMode, actual.SectionDeletionMode);

            public override string BuildString(IniDocumentConfiguration target)
                => $"{nameof(target.SectionDeletionMode)} = {target.SectionDeletionMode}";
        }
    }
}
