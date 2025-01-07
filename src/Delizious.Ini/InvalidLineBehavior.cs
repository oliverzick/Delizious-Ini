namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Specifies how an <see cref="IniDocument"/> should behave on loading when a line is invalid and cannot be parsed.
    /// </summary>
    public sealed class InvalidLineBehavior : IEquatable<InvalidLineBehavior>
    {
        private readonly IBehavior behavior;

        private InvalidLineBehavior(IBehavior behavior)
        {
            this.behavior = behavior;
        }

        /// <summary>
        /// Specifies that loading an <see cref="IniDocument"/> should fail
        /// by throwing an <see cref="PersistenceException"/>
        /// if a line is invalid and cannot be parsed.
        /// </summary>
        /// <returns>
        /// An <see cref="InvalidLineBehavior"/> instance that represents the fail mode.
        /// </returns>
        public static InvalidLineBehavior Fail
            => new InvalidLineBehavior(new FailBehavior());

        /// <summary>
        /// Specifies that loading an <see cref="IniDocument"/> should silently ignore
        /// if a line is invalid and cannot be parsed.
        /// </summary>
        /// <returns>
        /// An <see cref="InvalidLineBehavior"/> instance that represents the ignore mode.
        /// </returns>
        public static InvalidLineBehavior Ignore
            => new InvalidLineBehavior(new IgnoreBehavior());

        public static bool operator ==(InvalidLineBehavior left, InvalidLineBehavior right)
            => Equals(left, right);

        public static bool operator !=(InvalidLineBehavior left, InvalidLineBehavior right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(InvalidLineBehavior other)
            => !(other is null) && this.behavior.Equals(other.behavior);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as InvalidLineBehavior);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.behavior.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.behavior.ToString();

        internal T Transform<T>(IInvalidLineBehaviorTransformation<T> transformation)
            => this.behavior.Transform(transformation);

        private interface IBehavior
        {
            T Transform<T>(IInvalidLineBehaviorTransformation<T> transformation);

            bool Equals(IBehavior other);
        }

        private sealed class FailBehavior : IBehavior
        {
            public T Transform<T>(IInvalidLineBehaviorTransformation<T> transformation)
                => transformation.Fail();

            public bool Equals(IBehavior other)
                => other is FailBehavior;

            public override int GetHashCode()
                => nameof(FailBehavior).GetHashCode();

            public override string ToString()
                => "Fail";
        }

        private sealed class IgnoreBehavior : IBehavior
        {
            public T Transform<T>(IInvalidLineBehaviorTransformation<T> transformation)
                => transformation.Ignore();

            public bool Equals(IBehavior other)
                => other is IgnoreBehavior;

            public override int GetHashCode()
                => nameof(IgnoreBehavior).GetHashCode();

            public override string ToString()
                => "Ignore";
        }
    }

    internal interface IInvalidLineBehaviorTransformation<out T>
    {
        T Fail();

        T Ignore();
    }
}
