namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Represents the value of a property in an <see cref="IniDocument"/>.
    /// A property is also known as key-value pair.
    /// </summary>
    [Serializable]
    public sealed class PropertyValue : IEquatable<PropertyValue>
    {
        private readonly string propertyValue;

        private PropertyValue(string propertyValue)
        {
            this.propertyValue = propertyValue;
        }

        /// <summary>
        /// Creates a new instance of <see cref="PropertyValue"/> with the given <paramref name="propertyValue"/>.
        /// </summary>
        /// <param name="propertyValue">
        /// The value of a property.
        /// </param>
        /// <returns>
        /// A new <see cref="PropertyValue"/> instance that represents the given <paramref name="propertyValue"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyValue"/> is <c>null</c>.
        /// </exception>
        public static PropertyValue Create(string propertyValue)
        {
            if (propertyValue is null)
            {
                throw new ArgumentNullException(nameof(propertyValue));
            }

            return new PropertyValue(propertyValue);
        }

        public static implicit operator PropertyValue(string propertyValue)
            => Create(propertyValue);

        public static bool operator ==(PropertyValue left, PropertyValue right)
            => Equals(left, right);

        public static bool operator !=(PropertyValue left, PropertyValue right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(PropertyValue other)
        {
            if (other is null)
            {
                return false;
            }

            return this.propertyValue == other.propertyValue;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as PropertyValue);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.propertyValue.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.propertyValue;
    }
}
