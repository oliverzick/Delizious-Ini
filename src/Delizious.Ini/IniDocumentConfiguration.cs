namespace Delizious.Ini
{
    /// <summary>
    /// Represents the configuration of an <see cref="IniDocument"/>.
    /// </summary>
    public sealed class IniDocumentConfiguration
    {
        private IniDocumentConfiguration()
        {
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
        /// The mode that specifies how to read a property when the section or property does not exist.
        /// </summary>
        public PropertyReadMode PropertyReadMode { get; } = PropertyReadMode.Fail;

        /// <summary>
        /// The mode that specifies how to write a property.
        /// </summary>
        public PropertyWriteMode PropertyWriteMode { get; } = PropertyWriteMode.Update;
    }
}
