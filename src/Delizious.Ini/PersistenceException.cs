namespace Delizious.Ini
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when loading or saving an <see cref="IniDocument"/> failed.
    /// </summary>
    [Serializable]
    public sealed class PersistenceException : IniException
    {
        private PersistenceException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        internal static PersistenceException LoadingFailed(Exception innerException)
            => new PersistenceException(ExceptionMessages.CouldNotLoadIniDocument, innerException);

        internal static PersistenceException SavingFailed(Exception innerException)
            => new PersistenceException(ExceptionMessages.CouldNotSaveIniDocument, innerException);

        [ExcludeFromCodeCoverage]
        private PersistenceException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
