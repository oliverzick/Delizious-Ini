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
    }
}
