namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Specifies how an <see cref="IniDocument"/> should read a property.
    /// </summary>
    public sealed class PropertyReadMode : IEquatable<PropertyReadMode>
    {
        private readonly IMode mode;

        private PropertyReadMode(IMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Specifies that reading a property should fail by
        /// throwing a <see cref="SectionNotFoundException"/> when the section does not exist,
        /// or throwing a <see cref="PropertyNotFoundException"/> when the section exists but the property does not exist.
        /// </summary>
        /// <returns>
        /// A <see cref="PropertyReadMode"/> instance that represents the fail mode.
        /// </returns>
        public static PropertyReadMode Fail
            => new PropertyReadMode(new FailMode());

        /// <summary>
        /// Specifies that reading a property should fall back to <see cref="PropertyValue.None"/>
        /// when the section or property does not exist.
        /// </summary>
        /// <remarks>
        /// This is a convenience property that calls <see cref="CustomFallback(PropertyValue)"/> with <see cref="PropertyValue.None"/>.
        /// </remarks>
        /// <returns>
        /// A <see cref="PropertyReadMode"/> instance that represents the fallback mode with <see cref="PropertyValue.None"/>.
        /// </returns>
        public static PropertyReadMode Fallback
            => CustomFallback(PropertyValue.None);

        /// <summary>
        /// Specifies that reading a property should fall back to the given <paramref name="fallbackPropertyValue"/>
        /// when the section or property does not exist.
        /// </summary>
        /// <param name="fallbackPropertyValue">
        /// The property value to fall back when the section or property does not exist.
        /// </param>
        /// <returns>
        /// A <see cref="PropertyReadMode"/> instance that represents the fallback mode with the given <paramref name="fallbackPropertyValue"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fallbackPropertyValue"/> is <c>null</c>.
        /// </exception>
        public static PropertyReadMode CustomFallback(PropertyValue fallbackPropertyValue)
            => new PropertyReadMode(new FallbackMode(fallbackPropertyValue ?? throw new ArgumentNullException(nameof(fallbackPropertyValue))));

        public static bool operator ==(PropertyReadMode left, PropertyReadMode right)
            => Equals(left, right);

        public static bool operator !=(PropertyReadMode left, PropertyReadMode right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(PropertyReadMode other)
            => !(other is null) && this.mode.Equals(other.mode);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as PropertyReadMode);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.mode.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.mode.ToString();

        internal T Transform<T>(IPropertyReadModeTransformation<T> transformation)
            => this.mode.Transform(transformation);

        private interface IMode
        {
            T Transform<T>(IPropertyReadModeTransformation<T> transformation);

            bool Equals(IMode other);
        }

        private sealed class FailMode : IMode
        {
            public T Transform<T>(IPropertyReadModeTransformation<T> transformation)
                => transformation.Fail();

            public bool Equals(IMode other)
                => other is FailMode;

            public override int GetHashCode()
                => nameof(FailMode).GetHashCode();

            public override string ToString()
                => "Fail";
        }

        private sealed class FallbackMode : IMode
        {
            private readonly PropertyValue fallbackPropertyValue;

            public FallbackMode(PropertyValue fallbackPropertyValue)
            {
                this.fallbackPropertyValue = fallbackPropertyValue;
            }

            public T Transform<T>(IPropertyReadModeTransformation<T> transformation)
                => transformation.Fallback(this.fallbackPropertyValue);

            public bool Equals(IMode other)
                => other is FallbackMode otherMode && this.Equals(otherMode);

            private bool Equals(FallbackMode other)
                => this.fallbackPropertyValue.Equals(other.fallbackPropertyValue);

            public override int GetHashCode()
                => this.fallbackPropertyValue.GetHashCode();

            public override string ToString()
                => $"Fallback";
        }
    }

    internal interface IPropertyReadModeTransformation<out T>
    {
        T Fail();

        T Fallback(PropertyValue fallbackPropertyValue);
    }
}
