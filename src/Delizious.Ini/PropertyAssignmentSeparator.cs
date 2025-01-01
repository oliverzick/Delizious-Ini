namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Represents the assignment separator of a property in an <see cref="IniDocument"/>.
    /// </summary>
    public sealed class PropertyAssignmentSeparator : IEquatable<PropertyAssignmentSeparator>
    {
        private readonly char separator;

        private PropertyAssignmentSeparator(char separator)
        {
            this.separator = separator;
        }

        /// <summary>
        /// Creates a new instance of <see cref="PropertyAssignmentSeparator"/> with the given separator.
        /// </summary>
        /// <param name="separator">
        /// The assignment separator.
        /// </param>
        /// <returns>
        /// A new <see cref="PropertyAssignmentSeparator"/> instance that represents the given <paramref name="separator"/>.
        /// </returns>
        public static PropertyAssignmentSeparator Create(char separator)
            => new PropertyAssignmentSeparator(separator);

        public static implicit operator PropertyAssignmentSeparator(char separator)
            => Create(separator);

        /// <summary>
        /// The default assignment separator of a property which is the equality sign '<c>=</c>'.
        /// </summary>
        public static PropertyAssignmentSeparator Default
            => '=';

        public static bool operator ==(PropertyAssignmentSeparator left, PropertyAssignmentSeparator right)
            => Equals(left, right);

        public static bool operator !=(PropertyAssignmentSeparator left, PropertyAssignmentSeparator right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(PropertyAssignmentSeparator other)
            => !(other is null) && this.separator.Equals(other.separator);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as PropertyAssignmentSeparator);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.separator.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.separator.ToString();

        internal char ToChar()
            => this.separator;
    }
}
