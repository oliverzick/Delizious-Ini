namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Specifies how an <see cref="IniDocument"/> should delete a section.
    /// </summary>
    public sealed class SectionDeletionMode : IEquatable<SectionDeletionMode>
    {
        private readonly IMode mode;

        private SectionDeletionMode(IMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Specifies that deleting a section should fail by
        /// throwing a <see cref="SectionNotFoundException"/> if the section to delete does not exist.
        /// </summary>
        /// <returns>
        /// A <see cref="SectionDeletionMode"/> instance that represents the fail mode.
        /// </returns>
        public static SectionDeletionMode Fail
            => new SectionDeletionMode(new FailMode());

        /// <summary>
        /// Specifies that deleting a section should silently ignore if the section to delete does not exist.
        /// </summary>
        /// <returns>
        /// A <see cref="SectionDeletionMode"/> instance that represents the ignore mode.
        /// </returns>
        public static SectionDeletionMode Ignore
            => new SectionDeletionMode(new IgnoreMode());

        public static bool operator ==(SectionDeletionMode left, SectionDeletionMode right)
            => Equals(left, right);

        public static bool operator !=(SectionDeletionMode left, SectionDeletionMode right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(SectionDeletionMode other)
            => !(other is null) && this.mode.Equals(other.mode);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as SectionDeletionMode);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.mode.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.mode.ToString();

        internal T Transform<T>(ISectionDeletionModeTransformation<T> transformation)
            => this.mode.Transform(transformation);

        private interface IMode
        {
            T Transform<T>(ISectionDeletionModeTransformation<T> transformation);

            bool Equals(IMode other);
        }

        private sealed class FailMode : IMode
        {
            public T Transform<T>(ISectionDeletionModeTransformation<T> transformation)
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
            public T Transform<T>(ISectionDeletionModeTransformation<T> transformation)
                => transformation.Ignore();

            public bool Equals(IMode other)
                => other is IgnoreMode;

            public override int GetHashCode()
                => nameof(IgnoreMode).GetHashCode();

            public override string ToString()
                => "Ignore";
        }
    }

    internal interface ISectionDeletionModeTransformation<out T>
    {
        T Fail();

        T Ignore();
    }
}
