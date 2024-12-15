namespace Delizious.Ini.Test;

using System;

[TestClass]
public sealed class IniDocumentConfigurationSpec
{
    private static IniDocumentConfiguration Target => IniDocumentConfiguration.Default;

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
        [TestMethod]
        public void Throws_argument_null_exception_when_given_case_sensitivity_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithCaseSensitivity(null!));
        }
    }

    [TestClass]
    public sealed class WithPropertyEnumerationMode
    {
        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_enumeration_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyEnumerationMode(null!));
        }
    }

    [TestClass]
    public sealed class WithPropertyReadMode
    {
        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_read_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyReadMode(null!));
        }
    }

    [TestClass]
    public sealed class WithPropertyWriteMode
    {
        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_write_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyWriteMode(null!));
        }
    }

    [TestClass]
    public sealed class WithPropertyDeletionMode
    {
        [TestMethod]
        public void Throws_argument_null_exception_when_given_property_deletion_mode_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => Target.WithPropertyDeletionMode(null!));
        }
    }
}
