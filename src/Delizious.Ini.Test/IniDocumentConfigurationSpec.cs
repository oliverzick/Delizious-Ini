namespace Delizious.Ini.Test;

[TestClass]
public sealed class IniDocumentConfigurationSpec
{
    [TestClass]
    public sealed class Default
    {
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
    }
}
