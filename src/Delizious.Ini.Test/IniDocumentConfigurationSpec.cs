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
        Setting.PropertyEnumerationMode,
        Setting.PropertyReadMode,
        Setting.PropertyWriteMode,
        Setting.PropertyDeletionMode,
        Setting.SectionDeletionMode
    ];

    [TestClass]
    public sealed class Default
    {
        [TestMethod]
        public void Specifies_case_sensitivity_as_case_insensitive()
        {
            var expected = CaseSensitivity.CaseInsensitive;

            var target = IniDocumentConfiguration.Default;

            var actual = target.CaseSensitivity;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Specifies_property_enumeration_mode_as_fallback()
        {
            var expected = PropertyEnumerationMode.Fallback;

            var target = IniDocumentConfiguration.Default;

            var actual = target.PropertyEnumerationMode;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Specifies_property_read_mode_as_fallback()
        {
            var expected = PropertyReadMode.Fallback;

            var target = IniDocumentConfiguration.Default;

            var actual = target.PropertyReadMode;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Specifies_property_write_mode_as_create()
        {
            var expected = PropertyWriteMode.Create;

            var target = IniDocumentConfiguration.Default;

            var actual = target.PropertyWriteMode;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Specifies_property_deletion_mode_as_ignore()
        {
            var expected = PropertyDeletionMode.Ignore;

            var target = IniDocumentConfiguration.Default;

            var actual = target.PropertyDeletionMode;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Specifies_section_deletion_mode_as_ignore()
        {
            var expected = SectionDeletionMode.Ignore;

            var target = IniDocumentConfiguration.Default;

            var actual = target.SectionDeletionMode;

            Assert.AreEqual(expected, actual);
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
        private static IniDocumentConfiguration CaseSensitivityCaseSensitive => IniDocumentConfiguration.Default.WithCaseSensitivity(CaseSensitivity.CaseSensitive);
        private static IniDocumentConfiguration PropertyDeletionModeFail => IniDocumentConfiguration.Default.WithPropertyDeletionMode(PropertyDeletionMode.Fail);
        private static IniDocumentConfiguration PropertyEnumerationModeFail => IniDocumentConfiguration.Default.WithPropertyEnumerationMode(PropertyEnumerationMode.Fail);
        private static IniDocumentConfiguration PropertyReadModeFail => IniDocumentConfiguration.Default.WithPropertyReadMode(PropertyReadMode.Fail);
        private static IniDocumentConfiguration PropertyWriteModeUpdate => IniDocumentConfiguration.Default.WithPropertyWriteMode(PropertyWriteMode.Update);
        private static IniDocumentConfiguration SectionDeletionModeFail => IniDocumentConfiguration.Default.WithSectionDeletionMode(SectionDeletionMode.Fail);

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
            yield return [CaseSensitivityCaseSensitive];
            yield return [PropertyDeletionModeFail];
            yield return [PropertyEnumerationModeFail];
            yield return [PropertyReadModeFail];
            yield return [PropertyWriteModeUpdate];
            yield return [SectionDeletionModeFail];
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
            yield return [Null, CaseSensitivityCaseSensitive, false];
            yield return [Null, PropertyDeletionModeFail, false];
            yield return [Null, PropertyEnumerationModeFail, false];
            yield return [Null, PropertyReadModeFail, false];
            yield return [Null, PropertyWriteModeUpdate, false];
            yield return [Null, SectionDeletionModeFail, false];
        }

        public static IEnumerable<object[]> Equals_null_test_cases()
        {
            yield return [Default, Null, false];
            yield return [CaseSensitivityCaseSensitive, Null, false];
            yield return [PropertyDeletionModeFail, Null, false];
            yield return [PropertyEnumerationModeFail, Null, false];
            yield return [PropertyReadModeFail, Null, false];
            yield return [PropertyWriteModeUpdate, Null, false];
            yield return [SectionDeletionModeFail, Null, false];
        }

        public static IEnumerable<object[]> Equals_test_cases()
        {
            yield return [Default, Default, true];
            yield return [Default, CaseSensitivityCaseSensitive, false];
            yield return [Default, PropertyDeletionModeFail, false];
            yield return [Default, PropertyEnumerationModeFail, false];
            yield return [Default, PropertyReadModeFail, false];
            yield return [Default, PropertyWriteModeUpdate, false];
            yield return [Default, SectionDeletionModeFail, false];

            yield return [CaseSensitivityCaseSensitive, Default, false];
            yield return [CaseSensitivityCaseSensitive, CaseSensitivityCaseSensitive, true];
            yield return [CaseSensitivityCaseSensitive, PropertyDeletionModeFail, false];
            yield return [CaseSensitivityCaseSensitive, PropertyEnumerationModeFail, false];
            yield return [CaseSensitivityCaseSensitive, PropertyReadModeFail, false];
            yield return [CaseSensitivityCaseSensitive, PropertyWriteModeUpdate, false];
            yield return [CaseSensitivityCaseSensitive, SectionDeletionModeFail, false];

            yield return [PropertyDeletionModeFail, Default, false];
            yield return [PropertyDeletionModeFail, CaseSensitivityCaseSensitive, false];
            yield return [PropertyDeletionModeFail, PropertyDeletionModeFail, true];
            yield return [PropertyDeletionModeFail, PropertyEnumerationModeFail, false];
            yield return [PropertyDeletionModeFail, PropertyReadModeFail, false];
            yield return [PropertyDeletionModeFail, PropertyWriteModeUpdate, false];
            yield return [PropertyDeletionModeFail, SectionDeletionModeFail, false];

            yield return [PropertyEnumerationModeFail, Default, false];
            yield return [PropertyEnumerationModeFail, CaseSensitivityCaseSensitive, false];
            yield return [PropertyEnumerationModeFail, PropertyDeletionModeFail, false];
            yield return [PropertyEnumerationModeFail, PropertyEnumerationModeFail, true];
            yield return [PropertyEnumerationModeFail, PropertyReadModeFail, false];
            yield return [PropertyEnumerationModeFail, PropertyWriteModeUpdate, false];
            yield return [PropertyEnumerationModeFail, SectionDeletionModeFail, false];

            yield return [PropertyReadModeFail, Default, false];
            yield return [PropertyReadModeFail, CaseSensitivityCaseSensitive, false];
            yield return [PropertyReadModeFail, PropertyDeletionModeFail, false];
            yield return [PropertyReadModeFail, PropertyEnumerationModeFail, false];
            yield return [PropertyReadModeFail, PropertyReadModeFail, true];
            yield return [PropertyReadModeFail, PropertyWriteModeUpdate, false];
            yield return [PropertyReadModeFail, SectionDeletionModeFail, false];

            yield return [PropertyWriteModeUpdate, Default, false];
            yield return [PropertyWriteModeUpdate, CaseSensitivityCaseSensitive, false];
            yield return [PropertyWriteModeUpdate, PropertyDeletionModeFail, false];
            yield return [PropertyWriteModeUpdate, PropertyEnumerationModeFail, false];
            yield return [PropertyWriteModeUpdate, PropertyReadModeFail, false];
            yield return [PropertyWriteModeUpdate, PropertyWriteModeUpdate, true];
            yield return [PropertyWriteModeUpdate, SectionDeletionModeFail, false];

            yield return [SectionDeletionModeFail, Default, false];
            yield return [SectionDeletionModeFail, CaseSensitivityCaseSensitive, false];
            yield return [SectionDeletionModeFail, PropertyDeletionModeFail, false];
            yield return [SectionDeletionModeFail, PropertyEnumerationModeFail, false];
            yield return [SectionDeletionModeFail, PropertyReadModeFail, false];
            yield return [SectionDeletionModeFail, PropertyWriteModeUpdate, false];
            yield return [SectionDeletionModeFail, SectionDeletionModeFail, true];
        }

        public static IEnumerable<object[]> General_equals_test_cases()
        {
            yield return [Default, new(), false];
            yield return [CaseSensitivityCaseSensitive, new(), false];
            yield return [PropertyDeletionModeFail, new(), false];
            yield return [PropertyEnumerationModeFail, new(), false];
            yield return [PropertyReadModeFail, new(), false];
            yield return [PropertyWriteModeUpdate, new(), false];
            yield return [SectionDeletionModeFail, new(), false];
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
