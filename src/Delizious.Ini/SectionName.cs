namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Represents the name of a section in an <see cref="IniDocument"/>.
    /// </summary>
    [Serializable]
    public sealed class SectionName : IEquatable<SectionName>
    {
        private readonly string sectionName;

        private SectionName(string sectionName)
        {
            this.sectionName = sectionName;
        }

        /// <summary>
        /// Creates a new instance of <see cref="SectionName"/> with the given <paramref name="sectionName"/>.
        /// </summary>
        /// <param name="sectionName">
        /// The name of the section.
        /// </param>
        /// <returns>
        /// A new <see cref="SectionName"/> instance that represents the given <paramref name="sectionName"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sectionName"/> is <c>null</c>.
        /// </exception>
        public static SectionName Create(string sectionName)
        {
            if (sectionName is null)
            {
                throw new ArgumentNullException(nameof(sectionName));
            }

            return new SectionName(sectionName);
        }

        public static implicit operator SectionName(string sectionName)
            => Create(sectionName);

        public static bool operator ==(SectionName left, SectionName right)
            => Equals(left, right);

        public static bool operator !=(SectionName left, SectionName right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(SectionName other)
        {
            if (other is null)
            {
                return false;
            }

            return this.sectionName == other.sectionName;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as SectionName);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.sectionName.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.sectionName;
    }
}
