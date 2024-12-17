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

        public override string ToString()
            => this.strategy.ToString();

        private abstract record Strategy
        {
            public abstract void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual);
        }

        public static Setting CaseSensitivity
            => new(new CaseSensitivityAssertion());

        private sealed record CaseSensitivityAssertion : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.CaseSensitivity, actual.CaseSensitivity);
        }

        public static Setting PropertyEnumerationMode
            => new(new PropertyEnumerationModeAssertion());

        private sealed record PropertyEnumerationModeAssertion : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyEnumerationMode, actual.PropertyEnumerationMode);
        }

        public static Setting PropertyReadMode
            => new(new PropertyReadModeAssertion());

        private sealed record PropertyReadModeAssertion : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyReadMode, actual.PropertyReadMode);
        }

        public static Setting PropertyWriteMode
            => new(new PropertyWriteModeAssertion());

        private sealed record PropertyWriteModeAssertion : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyWriteMode, actual.PropertyWriteMode);
        }

        public static Setting PropertyDeletionMode
            => new(new PropertyDeletionModeAssertion());

        private sealed record PropertyDeletionModeAssertion : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyDeletionMode, actual.PropertyDeletionMode);
        }

        public static Setting SectionDeletionMode
            => new(new SectionDeletionModeAssertion());

        private sealed record SectionDeletionModeAssertion : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.SectionDeletionMode, actual.SectionDeletionMode);
        }
    }
}
