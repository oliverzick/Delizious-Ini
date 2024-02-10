namespace Delizious.Ini
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    [Serializable]
    public class IniException : Exception
    {
        internal IniException(string message)
            : base(message)
        {
        }

        internal IniException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        [ExcludeFromCodeCoverage]
        internal IniException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
