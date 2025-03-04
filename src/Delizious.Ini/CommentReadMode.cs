namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Specifies how an <see cref="IniDocument"/> should read a comment.
    /// </summary>
    public sealed class CommentReadMode : IEquatable<CommentReadMode>
    {
        private readonly IMode mode;

        private CommentReadMode(IMode mode)
        {
            this.mode = mode;
        }

        /// <summary>
        /// Specifies that reading a comment should fail by
        /// throwing a <see cref="SectionNotFoundException"/> when the section does not exist,
        /// or throwing a <see cref="PropertyNotFoundException"/> when the property does not exist.
        /// </summary>
        /// <returns>
        /// A <see cref="CommentReadMode"/> instance that represents the fail mode.
        /// </returns>
        public static CommentReadMode Fail
            => new CommentReadMode(new FailMode());

        /// <summary>
        /// Specifies that reading a comment should fall back to <see cref="Comment.None"/>
        /// when the section or property does not exist.
        /// </summary>
        /// <remarks>
        /// This is a convenience property that calls <see cref="CustomFallback(Comment)"/> with <see cref="Comment.None"/>.
        /// </remarks>
        /// <returns>
        /// A <see cref="CommentReadMode"/> instance that represents the fallback mode with <see cref="Comment.None"/>.
        /// </returns>
        public static CommentReadMode Fallback
            => CustomFallback(Comment.None);

        /// <summary>
        /// Specifies that reading a comment should fall back to the given <paramref name="fallbackComment"/>
        /// when the section or property does not exist.
        /// </summary>
        /// <param name="fallbackComment">
        /// The comment to fall back when the section or property does not exist.
        /// </param>
        /// <returns>
        /// A <see cref="CommentReadMode"/> instance that represents the fallback mode with the given <paramref name="fallbackComment"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fallbackComment"/> is <c>null</c>.
        /// </exception>
        public static CommentReadMode CustomFallback(Comment fallbackComment)
            => new CommentReadMode(new FallbackMode(fallbackComment ?? throw new ArgumentNullException(nameof(fallbackComment))));

        public static bool operator ==(CommentReadMode left, CommentReadMode right)
            => Equals(left, right);

        public static bool operator !=(CommentReadMode left, CommentReadMode right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(CommentReadMode other)
            => !(other is null) && this.mode.Equals(other.mode);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as CommentReadMode);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.mode.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.mode.ToString();

        internal T Transform<T>(ICommentReadModeTransformation<T> transformation)
            => this.mode.Transform(transformation);

        private interface IMode
        {
            T Transform<T>(ICommentReadModeTransformation<T> transformation);

            bool Equals(IMode other);
        }

        private sealed class FailMode : IMode
        {
            public T Transform<T>(ICommentReadModeTransformation<T> transformation)
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
            private readonly Comment fallbackComment;

            public FallbackMode(Comment fallbackComment)
            {
                this.fallbackComment = fallbackComment;
            }

            public T Transform<T>(ICommentReadModeTransformation<T> transformation)
                => transformation.Fallback(this.fallbackComment);

            public bool Equals(IMode other)
                => other is FallbackMode otherMode && this.Equals(otherMode);

            private bool Equals(FallbackMode other)
                => this.fallbackComment.Equals(other.fallbackComment);

            public override int GetHashCode()
                => this.fallbackComment.GetHashCode();

            public override string ToString()
                => "Fallback";
        }
    }

    internal interface ICommentReadModeTransformation<out T>
    {
        T Fail();

        T Fallback(Comment fallbackComment);
    }
}
