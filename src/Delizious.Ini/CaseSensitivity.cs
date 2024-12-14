namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Represents case sensitivity that indicates whether uppercase and lowercase letters are treated as equivalent (case-insensitive) or distinct (case-sensitive).
    /// </summary>
    public sealed class CaseSensitivity : IEquatable<CaseSensitivity>
    {
        private readonly IStrategy strategy;

        private CaseSensitivity(IStrategy strategy)
        {
            this.strategy = strategy;
        }

        /// <summary>
        /// Specifies that uppercase and lowercase letters are treated as equivalent.
        /// </summary>
        /// <returns>
        /// A <see cref="CaseSensitivity"/> instance that indicates treating uppercase and lowercase letters as equivalent.
        /// </returns>
        public static CaseSensitivity CaseInsensitive
            => new CaseSensitivity(new CaseInsensitiveStrategy());

        /// <summary>
        /// Specifies that uppercase and lowercase letters are treated as distinct.
        /// </summary>
        /// <returns>
        /// A <see cref="CaseSensitivity"/> instance that indicates treating uppercase and lowercase letters as distinct.
        /// </returns>
        public static CaseSensitivity CaseSensitive
            => new CaseSensitivity(new CaseSensitiveStrategy());

        public static bool operator ==(CaseSensitivity left, CaseSensitivity right)
            => Equals(left, right);

        public static bool operator !=(CaseSensitivity left, CaseSensitivity right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(CaseSensitivity other)
            => !(other is null) && this.strategy.Equals(other.strategy);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as CaseSensitivity);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.strategy.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.strategy.ToString();

        internal T Transform<T>(ICaseSensitivityTransformation<T> transformation)
            => this.strategy.Transform(transformation);

        private interface IStrategy
        {
            T Transform<T>(ICaseSensitivityTransformation<T> transformation);
            bool Equals(IStrategy other);
        }

        private sealed class CaseInsensitiveStrategy : IStrategy
        {
            public T Transform<T>(ICaseSensitivityTransformation<T> transformation)
                => transformation.CaseInsensitive();

            public bool Equals(IStrategy other)
                => other is CaseInsensitiveStrategy;

            public override int GetHashCode()
                => nameof(CaseInsensitiveStrategy).GetHashCode();

            public override string ToString()
                => "CaseInsensitive";
        }

        private sealed class CaseSensitiveStrategy : IStrategy
        {
            public T Transform<T>(ICaseSensitivityTransformation<T> transformation)
                => transformation.CaseSensitive();

            public bool Equals(IStrategy other)
                => other is CaseSensitiveStrategy;

            public override int GetHashCode()
                => nameof(CaseSensitiveStrategy).GetHashCode();

            public override string ToString()
                => "CaseSensitive";
        }
    }

    internal interface ICaseSensitivityTransformation<out T>
    {
        T CaseSensitive();

        T CaseInsensitive();
    }
}
