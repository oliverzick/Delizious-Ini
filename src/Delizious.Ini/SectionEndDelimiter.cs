namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Represents the end delimiter of a section in an <see cref="IniDocument"/>.
    /// </summary>
    public sealed class SectionEndDelimiter : IEquatable<SectionEndDelimiter>
    {
        private readonly char delimiter;

        private SectionEndDelimiter(char delimiter)
        {
            this.delimiter = delimiter;
        }

        /// <summary>
        /// Creates a new instance of <see cref="SectionEndDelimiter"/> with the given delimiter.
        /// </summary>
        /// <param name="delimiter">
        /// The delimiter.
        /// </param>
        /// <returns>
        /// A new <see cref="SectionEndDelimiter"/> instance that represents the given <paramref name="delimiter"/>.
        /// </returns>
        public static SectionEndDelimiter Create(char delimiter)
            => new SectionEndDelimiter(delimiter);

        public static implicit operator SectionEndDelimiter(char delimiter)
            => Create(delimiter);

        /// <summary>
        /// The default end delimiter of a section which is the closing square bracket '<c>]</c>'.
        /// </summary>
        public static SectionEndDelimiter Default
            => ']';

        public static bool operator ==(SectionEndDelimiter left, SectionEndDelimiter right)
            => Equals(left, right);

        public static bool operator !=(SectionEndDelimiter left, SectionEndDelimiter right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(SectionEndDelimiter other)
            => !(other is null) && this.delimiter.Equals(other.delimiter);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as SectionEndDelimiter);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.delimiter.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.delimiter.ToString();

        internal char ToChar()
            => this.delimiter;
    }
}
