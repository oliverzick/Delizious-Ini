namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Represents the assignment spacer that is used before and after the <see cref="PropertyAssignmentSeparator"/>.
    /// </summary>
    public sealed class PropertyAssignmentSpacer : IEquatable<PropertyAssignmentSpacer>
    {
        private readonly string spacer;

        private PropertyAssignmentSpacer(string spacer)
        {
            this.spacer = spacer;
        }

        /// <summary>
        /// Specifies that no assignment spacer is used.
        /// </summary>
        public static PropertyAssignmentSpacer None
            => new PropertyAssignmentSpacer(string.Empty);

        /// <summary>
        /// Specifies that a single space is used as assignment spacer.
        /// </summary>
        public static PropertyAssignmentSpacer Space
            => new PropertyAssignmentSpacer(" ");

        public static bool operator ==(PropertyAssignmentSpacer left, PropertyAssignmentSpacer right)
            => Equals(left, right);

        public static bool operator !=(PropertyAssignmentSpacer left, PropertyAssignmentSpacer right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(PropertyAssignmentSpacer other)
            => !(other is null) && this.spacer.Equals(other.spacer);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as PropertyAssignmentSpacer);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.spacer.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.spacer;
    }
}
