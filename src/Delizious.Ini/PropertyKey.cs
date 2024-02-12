namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Represents the key of a property in an <see cref="IniDocument"/>.
    /// A property is also known as key-value pair.
    /// </summary>
    [Serializable]
    public sealed class PropertyKey : IEquatable<PropertyKey>
    {
        private readonly string propertyKey;

        private PropertyKey(string propertyKey)
        {
            this.propertyKey = propertyKey;
        }

        /// <summary>
        /// Creates a new instance of <see cref="PropertyKey"/> with the given <paramref name="propertyKey"/>.
        /// </summary>
        /// <param name="propertyKey">
        /// The key of a property.
        /// </param>
        /// <returns>
        /// A new <see cref="PropertyKey"/> instance that represents the given <paramref name="propertyKey"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyKey"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="propertyKey"/> is either <see cref="string.Empty"/> or consists only of white-space characters.
        /// </exception>
        public static PropertyKey Create(string propertyKey)
        {
            if (propertyKey is null)
            {
                throw new ArgumentNullException(nameof(propertyKey));
            }

            if (string.IsNullOrWhiteSpace(propertyKey))
            {
                throw new ArgumentException(ExceptionMessages.PropertyKeyMustNotBeEmptyOrConsistOnlyOfWhiteSpaceCharacters, nameof(propertyKey));
            }

            return new PropertyKey(propertyKey);
        }

        public static implicit operator PropertyKey(string propertyKey)
            => Create(propertyKey);

        public static bool operator ==(PropertyKey left, PropertyKey right)
            => Equals(left, right);

        public static bool operator !=(PropertyKey left, PropertyKey right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(PropertyKey other)
        {
            if (other is null)
            {
                return false;
            }

            return this.propertyKey == other.propertyKey;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as PropertyKey);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.propertyKey.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.propertyKey;
    }
}
