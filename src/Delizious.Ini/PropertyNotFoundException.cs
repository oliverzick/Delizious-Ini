namespace Delizious.Ini
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;

    /// <summary>
    /// The exception that is thrown when a property cannot be found.
    /// </summary>
    [Serializable]
    public sealed class PropertyNotFoundException : IniException
    {
        internal PropertyNotFoundException(PropertyKey propertyKey)
            : base(ExceptionMessages.CouldNotFindPropertyInIniDocument)
        {
            this.PropertyKey = propertyKey;
        }

        [ExcludeFromCodeCoverage]
        private PropertyNotFoundException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Gets the key of the property that was not found.
        /// </summary>
        public PropertyKey PropertyKey { get; }
    }
}
