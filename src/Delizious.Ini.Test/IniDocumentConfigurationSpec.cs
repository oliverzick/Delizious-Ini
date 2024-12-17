namespace Delizious.Ini.Test;

using System;
using System.Collections.Generic;
using System.Linq;

[TestClass]
public sealed class IniDocumentConfigurationSpec
{
    private static IniDocumentConfiguration Target => IniDocumentConfiguration.Default;

    public static IEnumerable<Assertion> AllAssertions =>
    [
        Assertion.CaseSensitivity,
        Assertion.PropertyEnumerationMode,
        Assertion.PropertyReadMode,
        Assertion.PropertyWriteMode,
        Assertion.PropertyDeletionMode,
        Assertion.SectionDeletionMode
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
        private static IEnumerable<Assertion> AssertionsToExclude => [Assertion.CaseSensitivity];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_case_sensitivity_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithCaseSensitivity(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Assertion assertion)
        {
            var original = Target;

            var actual = original.WithCaseSensitivity(CaseSensitivity.CaseSensitive);

            assertion.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllAssertions.Except(AssertionsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyEnumerationMode
    {
        private static IEnumerable<Assertion> AssertionsToExclude => [Assertion.PropertyEnumerationMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_enumeration_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyEnumerationMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Assertion assertion)
        {
            var original = Target;

            var actual = original.WithPropertyEnumerationMode(PropertyEnumerationMode.Fail);

            assertion.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllAssertions.Except(AssertionsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyReadMode
    {
        private static IEnumerable<Assertion> AssertionsToExclude => [Assertion.PropertyReadMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_read_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyReadMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Assertion assertion)
        {
            var original = Target;

            var actual = original.WithPropertyReadMode(PropertyReadMode.Fail);

            assertion.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllAssertions.Except(AssertionsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyWriteMode
    {
        private static IEnumerable<Assertion> AssertionsToExclude => [Assertion.PropertyWriteMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_write_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyWriteMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Assertion assertion)
        {
            var original = Target;

            var actual = original.WithPropertyWriteMode(PropertyWriteMode.Update);

            assertion.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllAssertions.Except(AssertionsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithPropertyDeletionMode
    {
        private static IEnumerable<Assertion> AssertionsToExclude => [Assertion.PropertyDeletionMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_deletion_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyDeletionMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Assertion assertion)
        {
            var original = Target;

            var actual = original.WithPropertyDeletionMode(PropertyDeletionMode.Fail);

            assertion.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllAssertions.Except(AssertionsToExclude).Select(ToTestCase);
    }

    [TestClass]
    public sealed class WithSectionDeletionMode
    {
        private static IEnumerable<Assertion> AssertionsToExclude => [Assertion.SectionDeletionMode];

        [TestMethod]
        public void Throws_argument_null_exception_when_given_section_deletion_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithSectionDeletionMode(null!));
        }

        [DataTestMethod]
        [DynamicData(nameof(Retains_remaining_settings_test_cases), DynamicDataSourceType.Method)]
        public void Retains_remaining_settings(Assertion assertion)
        {
            var original = Target;

            var actual = original.WithSectionDeletionMode(SectionDeletionMode.Fail);

            assertion.AssertIsEqual(original, actual);
        }

        public static IEnumerable<object[]> Retains_remaining_settings_test_cases()
            => AllAssertions.Except(AssertionsToExclude).Select(ToTestCase);
    }

    private static object[] ToTestCase(Assertion assertion)
        => [assertion];

    public sealed record Assertion
    {
        private readonly Strategy strategy;

        private Assertion(Strategy strategy)
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

        public static Assertion CaseSensitivity
            => new(new CaseSensitivityAssertion());

        private sealed record CaseSensitivityAssertion : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.CaseSensitivity, actual.CaseSensitivity);
        }

        public static Assertion PropertyEnumerationMode
            => new(new PropertyEnumerationModeAssertion());

        private sealed record PropertyEnumerationModeAssertion : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyEnumerationMode, actual.PropertyEnumerationMode);
        }

        public static Assertion PropertyReadMode
            => new(new PropertyReadModeAssertion());

        private sealed record PropertyReadModeAssertion : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyReadMode, actual.PropertyReadMode);
        }

        public static Assertion PropertyWriteMode
            => new(new PropertyWriteModeAssertion());

        private sealed record PropertyWriteModeAssertion : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyWriteMode, actual.PropertyWriteMode);
        }

        public static Assertion PropertyDeletionMode
            => new(new PropertyDeletionModeAssertion());

        private sealed record PropertyDeletionModeAssertion : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.PropertyDeletionMode, actual.PropertyDeletionMode);
        }

        public static Assertion SectionDeletionMode
            => new(new SectionDeletionModeAssertion());

        private sealed record SectionDeletionModeAssertion : Strategy
        {
            public override void AssertIsEqual(IniDocumentConfiguration original, IniDocumentConfiguration actual)
                => Assert.AreEqual(original.SectionDeletionMode, actual.SectionDeletionMode);
        }
    }
}
