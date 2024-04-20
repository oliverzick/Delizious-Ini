namespace Delizious.Ini
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when the serialization of an <see cref="IniDocument"/> failed.
    /// </summary>
    [Serializable]
    public sealed class SerializationException : IniException
    {
        private SerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        internal static SerializationException ForSerialization(Exception innerException)
            => new SerializationException(ExceptionMessages.CouldNotSerializeIniDocument, innerException);

        [ExcludeFromCodeCoverage]
        private SerializationException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
