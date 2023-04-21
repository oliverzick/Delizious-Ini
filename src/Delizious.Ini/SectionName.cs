namespace Delizious.Ini
{
    using System;

    public sealed class SectionName : IEquatable<SectionName>
    {
        private readonly string sectionName;

        private SectionName(string sectionName)
        {
            this.sectionName = sectionName;
        }

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

        public bool Equals(SectionName other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            return this.sectionName == other.sectionName;
        }

        public override bool Equals(object obj)
            => this.Equals(obj as SectionName);

        public override int GetHashCode()
            => this.sectionName.GetHashCode();

        public override string ToString()
            => this.sectionName;
    }
}
