namespace Delizious.Ini
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [Serializable]
    public sealed class IniException : Exception
    {
        internal IniException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [ExcludeFromCodeCoverage]
        private IniException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
