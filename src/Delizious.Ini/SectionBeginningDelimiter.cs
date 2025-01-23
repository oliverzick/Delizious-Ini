namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Represents the beginning delimiter of a section in an <see cref="IniDocument"/>.
    /// </summary>
    public sealed class SectionBeginningDelimiter : IEquatable<SectionBeginningDelimiter>
    {
        private readonly char delimiter;

        private SectionBeginningDelimiter(char delimiter)
        {
            this.delimiter = delimiter;
        }

        /// <summary>
        /// Creates a new instance of <see cref="SectionBeginningDelimiter"/> with the given delimiter.
        /// </summary>
        /// <param name="delimiter">
        /// The delimiter.
        /// </param>
        /// <returns>
        /// A new <see cref="SectionBeginningDelimiter"/> instance that represents the given <paramref name="delimiter"/>.
        /// </returns>
        public static SectionBeginningDelimiter Create(char delimiter)
            => new SectionBeginningDelimiter(delimiter);

        public static implicit operator SectionBeginningDelimiter(char delimiter)
            => Create(delimiter);

        /// <summary>
        /// The default beginning delimiter of a section which is the opening square bracket '<c>[</c>'.
        /// </summary>
        public static SectionBeginningDelimiter Default
            => '[';

        public static bool operator ==(SectionBeginningDelimiter left, SectionBeginningDelimiter right)
            => Equals(left, right);

        public static bool operator !=(SectionBeginningDelimiter left, SectionBeginningDelimiter right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(SectionBeginningDelimiter other)
            => !(other is null) && this.delimiter.Equals(other.delimiter);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as SectionBeginningDelimiter);

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
