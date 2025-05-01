namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Represents the string that indicates the beginning of a comment line in an <see cref="IniDocument"/>.
    /// </summary>
    [Serializable]
    public sealed class CommentString : IEquatable<CommentString>
    {
        private readonly string commentString;

        private CommentString(string commentString)
        {
            this.commentString = commentString;
        }

        /// <summary>
        /// Creates a new instance of <see cref="CommentString"/> with the given <paramref name="commentString"/>.
        /// </summary>
        /// <param name="commentString">
        /// The comment string.
        /// </param>
        /// <returns>
        /// A new <see cref="CommentString"/> instance that represents the given <paramref name="commentString"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="commentString"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="commentString"/> is either <see cref="string.Empty"/> or consists only of white-space characters.
        /// </exception>
        public static CommentString Create(string commentString)
        {
            if (commentString is null)
            {
                throw new ArgumentNullException(nameof(commentString));
            }

            if (string.IsNullOrWhiteSpace(commentString))
            {
                throw new ArgumentException(ExceptionMessages.CommentStringMustNotBeEmptyOrConsistOnlyOfWhiteSpaceCharacters, nameof(commentString));
            }

            return new CommentString(commentString);
        }

        public static implicit operator CommentString(string comment)
            => Create(comment);

        /// <summary>
        /// The default string that indicates the beginning of a comment line which is a semicolon '<c>;</c>'.
        /// </summary>
        public static CommentString Default
            => ";";

        public static bool operator ==(CommentString left, CommentString right)
            => Equals(left, right);

        public static bool operator !=(CommentString left, CommentString right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(CommentString other)
        {
            if (other is null)
            {
                return false;
            }

            return this.commentString == other.commentString;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as CommentString);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.commentString.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.commentString;
    }
}
