namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Specifies how an <see cref="IniDocument"/> should behave on loading when a duplicate property occurs.
    /// </summary>
    public sealed class DuplicatePropertyBehavior : IEquatable<DuplicatePropertyBehavior>
    {
        private readonly IBehavior behavior;

        private DuplicatePropertyBehavior(IBehavior behavior)
        {
            this.behavior = behavior;
        }

        /// <summary>
        /// Specifies that loading an <see cref="IniDocument"/> should fail
        /// by throwing an <see cref="PersistenceException"/>
        /// if a duplicate property occurs.
        /// </summary>
        /// <returns>
        /// A <see cref="DuplicatePropertyBehavior"/> instance that represents the fail behavior.
        /// </returns>
        public static DuplicatePropertyBehavior Fail
            => new DuplicatePropertyBehavior(new FailBehavior());

        /// <summary>
        /// Specifies that loading an <see cref="IniDocument"/> should ignore subsequent occurrences of a duplicate property and discard their values
        /// by using the first occurrence of such a property.
        /// </summary>
        /// <returns>
        /// A <see cref="DuplicatePropertyBehavior"/> instance that represents the ignore behavior.
        /// </returns>
        public static DuplicatePropertyBehavior Ignore
            => new DuplicatePropertyBehavior(new IgnoreBehavior());

        /// <summary>
        /// Specifies that loading an <see cref="IniDocument"/> should override previous occurrences of a duplicate property and discard their values
        /// by using the last occurrence of such a property.
        /// </summary>
        /// <returns>
        /// A <see cref="DuplicatePropertyBehavior"/> instance that represents the override behavior.
        /// </returns>
        public static DuplicatePropertyBehavior Override
            => new DuplicatePropertyBehavior(new OverrideBehavior());

        public static bool operator ==(DuplicatePropertyBehavior left, DuplicatePropertyBehavior right)
            => Equals(left, right);

        public static bool operator !=(DuplicatePropertyBehavior left, DuplicatePropertyBehavior right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(DuplicatePropertyBehavior other)
            => !(other is null) && this.behavior.Equals(other.behavior);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as DuplicatePropertyBehavior);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.behavior.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.behavior.ToString();

        internal T Transform<T>(IDuplicatePropertyBehaviorTransformation<T> transformation)
            => this.behavior.Transform(transformation);

        private interface IBehavior
        {
            T Transform<T>(IDuplicatePropertyBehaviorTransformation<T> transformation);

            bool Equals(IBehavior other);
        }

        private sealed class FailBehavior : IBehavior
        {
            public T Transform<T>(IDuplicatePropertyBehaviorTransformation<T> transformation)
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
            public T Transform<T>(IDuplicatePropertyBehaviorTransformation<T> transformation)
                => transformation.Ignore();

            public bool Equals(IBehavior other)
                => other is IgnoreBehavior;

            public override int GetHashCode()
                => nameof(IgnoreBehavior).GetHashCode();

            public override string ToString()
                => "Ignore";
        }

        private sealed class OverrideBehavior : IBehavior
        {
            public T Transform<T>(IDuplicatePropertyBehaviorTransformation<T> transformation)
                => transformation.Override();

            public bool Equals(IBehavior other)
                => other is OverrideBehavior;

            public override int GetHashCode()
                => nameof(OverrideBehavior).GetHashCode();

            public override string ToString()
                => "Override";
        }
    }

    internal interface IDuplicatePropertyBehaviorTransformation<out T>
    {
        T Fail();

        T Ignore();

        T Override();
    }
}
