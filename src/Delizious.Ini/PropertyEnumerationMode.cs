namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Specifies how an <see cref="IniDocument"/> should enumerate the properties contained in a section.
    /// </summary>
    public sealed class PropertyEnumerationMode : IEquatable<PropertyEnumerationMode>
    {
        private readonly IMode mode;

        private PropertyEnumerationMode(IMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Specifies that enumerating the properties contained in a section should fail and throw a <see cref="SectionNotFoundException"/>
        /// when the section does not exist.
        /// </summary>
        /// <returns>
        /// A <see cref="PropertyEnumerationMode"/> instance that represents the fail mode.
        /// </returns>
        public static PropertyEnumerationMode Fail()
            => new PropertyEnumerationMode(new FailMode());

        /// <summary>
        /// Specifies that enumerating the properties contained in a section should fall back to an empty collection of properties
        /// when the section does not exist.
        /// </summary>
        /// <returns>
        /// A <see cref="PropertyEnumerationMode"/> instance that represents the fallback mode.
        /// </returns>
        public static PropertyEnumerationMode Fallback
            => new PropertyEnumerationMode(new FallbackMode());

        public static bool operator ==(PropertyEnumerationMode left, PropertyEnumerationMode right)
            => Equals(left, right);

        public static bool operator !=(PropertyEnumerationMode left, PropertyEnumerationMode right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(PropertyEnumerationMode other)
            => !(other is null) && this.mode.Equals(other.mode);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as PropertyEnumerationMode);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.mode.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.mode.ToString();

        internal T Transform<T>(IPropertyEnumerationModeTransformation<T> transformation)
            => this.mode.Transform(transformation);

        private interface IMode
        {
            T Transform<T>(IPropertyEnumerationModeTransformation<T> transformation);

            bool Equals(IMode mode);
        }

        private sealed class FailMode : IMode
        {
            public T Transform<T>(IPropertyEnumerationModeTransformation<T> transformation)
                => transformation.Fail();

            public bool Equals(IMode mode)
                => mode is FailMode;

            public override int GetHashCode()
                => nameof(FailMode).GetHashCode();

            public override string ToString()
                => "Fail";
        }

        private sealed class FallbackMode : IMode
        {
            public T Transform<T>(IPropertyEnumerationModeTransformation<T> transformation)
                => transformation.Fallback();

            public bool Equals(IMode mode)
                => mode is FallbackMode;

            public override int GetHashCode()
                => nameof(FallbackMode).GetHashCode();

            public override string ToString()
                => "Fallback";
        }
    }

    internal interface IPropertyEnumerationModeTransformation<out T>
    {
        T Fail();

        T Fallback();
    }
}
