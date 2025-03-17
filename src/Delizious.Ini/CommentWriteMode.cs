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
        /// Specifies that writing a comment should create a new comment.
        /// If a comment already exists, it will be overwritten.
        /// </summary>
        /// <returns>
        /// A <see cref="CommentWriteMode"/> instance that represents the create mode.
        /// </returns>
        public static CommentWriteMode Create
            => new CommentWriteMode(new CreateMode());

        /// <summary>
        /// Specifies that writing a comment should update an existing comment and requires that both the section and property exist.
        /// </summary>
        /// <returns>
        /// A <see cref="CommentWriteMode"/> instance that represents the update mode.
        /// </returns>
        public static CommentWriteMode Update
            => new CommentWriteMode(new UpdateMode());

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

        private sealed class CreateMode : IMode
        {
            public T Transform<T>(ICommentWriteModeTransformation<T> transformation)
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
            public T Transform<T>(ICommentWriteModeTransformation<T> transformation)
                => transformation.Update();

            public bool Equals(IMode other)
                => other is UpdateMode;

            public override int GetHashCode()
                => nameof(UpdateMode).GetHashCode();

            public override string ToString()
                => "Update";
        }
    }

    internal interface ICommentWriteModeTransformation<out T>
    {
        T Create();

        T Update();
    }
}
