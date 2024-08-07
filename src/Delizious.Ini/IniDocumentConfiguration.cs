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
    }
}
