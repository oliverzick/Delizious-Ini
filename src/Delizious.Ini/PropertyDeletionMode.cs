namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Specifies how an <see cref="IniDocument"/> should delete a property.
    /// </summary>
    public sealed class PropertyDeletionMode : IEquatable<PropertyDeletionMode>
    {
        private readonly IMode mode;

        private PropertyDeletionMode(IMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Specifies that deleting a property should fail by throwing an appropriate exception.
        /// Throws a <see cref="SectionNotFoundException"/> if the section does not exist.
        /// Throws a <see cref="PropertyNotFoundException"/> if the section exists but the property to delete does not exist.
        /// </summary>
        /// <returns>
        /// A <see cref="PropertyDeletionMode"/> instance that represents the fail mode.
        /// </returns>
        public static PropertyDeletionMode Fail
            => new PropertyDeletionMode(new FailMode());

        /// <summary>
        /// Specifies that deleting a property should silently ignore if the section or the property to delete does not exist.
        /// </summary>
        /// <returns>
        /// A <see cref="PropertyDeletionMode"/> instance that represents the ignore mode.
        /// </returns>
        public static PropertyDeletionMode Ignore
            => new PropertyDeletionMode(new IgnoreMode());

        public static bool operator ==(PropertyDeletionMode left, PropertyDeletionMode right)
            => Equals(left, right);

        public static bool operator !=(PropertyDeletionMode left, PropertyDeletionMode right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(PropertyDeletionMode other)
            => !(other is null) && this.mode.Equals(other.mode);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as PropertyDeletionMode);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.mode.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.mode.ToString();

        internal T Transform<T>(IPropertyDeletionModeTransformation<T> transformation)
            => this.mode.Transform(transformation);

        private interface IMode
        {
            T Transform<T>(IPropertyDeletionModeTransformation<T> transformation);

            bool Equals(IMode other);
        }

        private sealed class FailMode : IMode
        {
            public T Transform<T>(IPropertyDeletionModeTransformation<T> transformation)
                => transformation.Fail();

            public bool Equals(IMode other)
                => other is FailMode;

            public override int GetHashCode()
                => nameof(FailMode).GetHashCode();

            public override string ToString()
                => "Fail";
        }

        private sealed class IgnoreMode : IMode
        {
            public T Transform<T>(IPropertyDeletionModeTransformation<T> transformation)
                => transformation.Ignore();

            public bool Equals(IMode other)
                => other is IgnoreMode;

            public override int GetHashCode()
                => nameof(IgnoreMode).GetHashCode();

            public override string ToString()
                => "Ignore";
        }
    }

    internal interface IPropertyDeletionModeTransformation<out T>
    {
        T Fail();

        T Ignore();
    }
}
