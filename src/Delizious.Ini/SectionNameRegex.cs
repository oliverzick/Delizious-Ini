namespace Delizious.Ini
{
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents the regular expression for the name of a section in an <see cref="IniDocument"/>.
    /// </summary>
    public sealed class SectionNameRegex : IEquatable<SectionNameRegex>
    {
        private readonly string pattern;

        private SectionNameRegex(string pattern)
        {
            this.pattern = pattern;
        }

        /// <summary>
        /// Creates a new instance of <see cref="SectionNameRegex"/> with the given <paramref name="pattern"/>.
        /// </summary>
        /// <param name="pattern">
        /// The regular expression pattern for the name of a section.
        /// </param>
        /// <returns>
        /// A new <see cref="SectionNameRegex"/> instance that represents the given <paramref name="pattern"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="pattern"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <para><paramref name="pattern"/> is <see cref="string.Empty"/>.</para>
        /// <para>- or -</para>
        /// <para><paramref name="pattern"/> is an invalid regular expression pattern.</para>
        /// </exception>
        public static SectionNameRegex Create(string pattern)
        {
            if (pattern is null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException(ExceptionMessages.PatternForSectionNameRegexMustNotBeEmpty, nameof(pattern));
            }

            try
            {
                _ = new Regex(pattern);
            }
            catch (ArgumentException exception)
            {
                throw new ArgumentException(ExceptionMessages.InvalidPatternForSectionNameRegex, nameof(pattern), exception);
            }

            return new SectionNameRegex(pattern);
        }

        /// <summary>
        /// The default section regex which is '<c>[\p{L}\p{M}\p{N}\p{P}\p{S}\p{Zs}]+</c>'.
        /// </summary>
        public static SectionNameRegex Default
            => Create(@"[\p{L}\p{M}\p{N}\p{P}\p{S}\p{Zs}]+");

        public static bool operator ==(SectionNameRegex left, SectionNameRegex right)
            => Equals(left, right);

        public static bool operator !=(SectionNameRegex left, SectionNameRegex right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(SectionNameRegex other)
            => !(other is null) && this.pattern == other.pattern;

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as SectionNameRegex);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.pattern.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.pattern;
    }
}
