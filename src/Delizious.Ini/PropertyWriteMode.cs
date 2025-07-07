namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Specifies how an <see cref="IniDocument"/> should write a property.
    /// </summary>
    public sealed class PropertyWriteMode : IEquatable<PropertyWriteMode>
    {
        private readonly IMode mode;

        private PropertyWriteMode(IMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Specifies that writing a property should create a new property.
        /// If the property already exists, it will be overwritten.
        /// If the section does not exist, a new section is created.
        /// If the section exists but the property itself does not exist, a new property is created.
        /// </summary>
        /// <returns>
        /// A <see cref="PropertyWriteMode"/> instance that represents the create mode.
        /// </returns>
        public static PropertyWriteMode Create
            => new PropertyWriteMode(new CreateMode());

        /// <summary>
        /// Specifies that writing a property should update an existing property and requires that both the section and property exist.
        /// Throws a <see cref="SectionNotFoundException"/> when the section does not exist,
        /// or throws a <see cref="PropertyNotFoundException"/> when the section exists but the property does not exist.
        /// </summary>
        /// <returns>
        /// A <see cref="PropertyWriteMode"/> instance that represents the update mode.
        /// </returns>
        public static PropertyWriteMode Update
            => new PropertyWriteMode(new UpdateMode());

        public static bool operator ==(PropertyWriteMode left, PropertyWriteMode right)
            => Equals(left, right);

        public static bool operator !=(PropertyWriteMode left, PropertyWriteMode right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(PropertyWriteMode other)
            => !(other is null) && this.mode.Equals(other.mode);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as PropertyWriteMode);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.mode.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.mode.ToString();

        internal T Transform<T>(IPropertyWriteModeTransformation<T> transformation)
            => this.mode.Transform(transformation);

        private interface IMode
        {
            T Transform<T>(IPropertyWriteModeTransformation<T> transformation);

            bool Equals(IMode other);
        }

        private sealed class CreateMode : IMode
        {
            public T Transform<T>(IPropertyWriteModeTransformation<T> transformation)
                => transformation.Create();

            public bool Equals(IMode other)
                => other is CreateMode;

            public override int GetHashCode()
                => nameof(CreateMode).GetHashCode();

            public override string ToString()
                => "Create";
        }

        private sealed class UpdateMode : IMode
        {
            public T Transform<T>(IPropertyWriteModeTransformation<T> transformation)
                => transformation.Update();

            public bool Equals(IMode other)
                => other is UpdateMode;

            public override int GetHashCode()
                => nameof(UpdateMode).GetHashCode();

            public override string ToString()
                => "Update";
        }
    }

    internal interface IPropertyWriteModeTransformation<out T>
    {
        T Create();

        T Update();
    }
}
