namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Specifies how an <see cref="IniDocument"/> should write a comment.
    /// </summary>
    public sealed class CommentWriteMode : IEquatable<CommentWriteMode>
    {
        private readonly IMode mode;

        private CommentWriteMode(IMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Specifies that writing a comment should fail by
        /// throwing a <see cref="SectionNotFoundException"/> when the section does not exist,
        /// or throwing a <see cref="PropertyNotFoundException"/> when the property does not exist.
        /// </summary>
        /// <returns>
        /// A <see cref="CommentWriteMode"/> instance that represents the fail mode.
        /// </returns>
        public static CommentWriteMode Fail
            => new CommentWriteMode(new FailMode());

        /// <summary>
        /// Specifies that writing a comment should silently ignore if the section or the property to write the comment does not exist.
        /// </summary>
        /// <returns>
        /// A <see cref="CommentWriteMode"/> instance that represents the ignore mode.
        /// </returns>
        public static CommentWriteMode Ignore
            => new CommentWriteMode(new IgnoreMode());

        public static bool operator ==(CommentWriteMode left, CommentWriteMode right)
            => Equals(left, right);

        public static bool operator !=(CommentWriteMode left, CommentWriteMode right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(CommentWriteMode other)
            => !(other is null) && this.mode.Equals(other.mode);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as CommentWriteMode);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.mode.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.mode.ToString();

        internal T Transform<T>(ICommentWriteModeTransformation<T> transformation)
            => this.mode.Transform(transformation);

        private interface IMode
        {
            T Transform<T>(ICommentWriteModeTransformation<T> transformation);

            bool Equals(IMode other);
        }

        private sealed class FailMode : IMode
        {
            public T Transform<T>(ICommentWriteModeTransformation<T> transformation)
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
            public T Transform<T>(ICommentWriteModeTransformation<T> transformation)
                => transformation.Ignore();

            public bool Equals(IMode other)
                => other is IgnoreMode;

            public override int GetHashCode()
                => nameof(IgnoreMode).GetHashCode();

            public override string ToString()
                => "Ignore";
        }
    }

    internal interface ICommentWriteModeTransformation<out T>
    {
        T Fail();

        T Ignore();
    }
}
