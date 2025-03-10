namespace Delizious.Ini
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents a comment in an <see cref="IniDocument"/>.
    /// </summary>
    [Serializable]
    public sealed class Comment : IEquatable<Comment>
    {
        private readonly string comment;

        private Comment(string comment)
        {
            this.comment = comment;
        }

        /// <summary>
        /// Creates a new instance of <see cref="Comment"/> with the given <paramref name="comment"/>.
        /// </summary>
        /// <param name="comment">
        /// The comment.
        /// </param>
        /// <returns>
        /// A new <see cref="Comment"/> instance that represents the given <paramref name="comment"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="comment"/> is <c>null</c>.
        /// </exception>
        public static Comment Create(string comment)
        {
            if (comment is null)
            {
                throw new ArgumentNullException(nameof(comment));
            }

            return new Comment(comment);
        }

        internal static Comment Create(IEnumerable<string> comments)
            => string.Join(Environment.NewLine, comments);

        public static implicit operator Comment(string comment)
            => Create(comment);

        /// <summary>
        /// Specifies that no comment is given.
        /// </summary>
        public static Comment None
            => string.Empty;

        public static bool operator ==(Comment left, Comment right)
            => Equals(left, right);

        public static bool operator !=(Comment left, Comment right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(Comment other)
        {
            if (other is null)
            {
                return false;
            }

            return this.comment == other.comment;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as Comment);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.comment.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.comment;
    }
}
