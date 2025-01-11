namespace Delizious.Ini
{
    using System;

    /// <summary>
    /// Specifies how an <see cref="IniDocument"/> should behave on loading when a duplicate section occurs.
    /// </summary>
    public sealed class DuplicateSectionBehavior : IEquatable<DuplicateSectionBehavior>
    {
        private readonly IBehavior behavior;

        private DuplicateSectionBehavior(IBehavior behavior)
        {
            this.behavior = behavior;
        }

        /// <summary>
        /// Specifies that loading an <see cref="IniDocument"/> should fail
        /// by throwing an <see cref="PersistenceException"/>
        /// if a duplicate section occurs.
        /// </summary>
        /// <returns>
        /// A <see cref="DuplicateSectionBehavior"/> instance that represents the fail behavior.
        /// </returns>
        public static DuplicateSectionBehavior Fail
            => new DuplicateSectionBehavior(new FailBehavior());

        /// <summary>
        /// Specifies that loading an <see cref="IniDocument"/> should merge a duplicate section.
        /// </summary>
        /// <returns>
        /// A <see cref="DuplicateSectionBehavior"/> instance that represents the merge behavior.
        /// </returns>
        public static DuplicateSectionBehavior Merge
            => new DuplicateSectionBehavior(new MergeBehavior());

        public static bool operator ==(DuplicateSectionBehavior left, DuplicateSectionBehavior right)
            => Equals(left, right);

        public static bool operator !=(DuplicateSectionBehavior left, DuplicateSectionBehavior right)
            => !(left == right);

        /// <inheritdoc/>
        public bool Equals(DuplicateSectionBehavior other)
            => !(other is null) && this.behavior.Equals(other.behavior);

        /// <inheritdoc/>
        public override bool Equals(object obj)
            => this.Equals(obj as DuplicateSectionBehavior);

        /// <inheritdoc/>
        public override int GetHashCode()
            => this.behavior.GetHashCode();

        /// <inheritdoc/>
        public override string ToString()
            => this.behavior.ToString();

        internal T Transform<T>(IDuplicateSectionBehaviorTransformation<T> transformation)
            => this.behavior.Transform(transformation);

        private interface IBehavior
        {
            T Transform<T>(IDuplicateSectionBehaviorTransformation<T> transformation);

            bool Equals(IBehavior other);
        }

        private sealed class FailBehavior : IBehavior
        {
            public T Transform<T>(IDuplicateSectionBehaviorTransformation<T> transformation)
                => transformation.Fail();

            public bool Equals(IBehavior other)
                => other is FailBehavior;

            public override int GetHashCode()
                => nameof(FailBehavior).GetHashCode();

            public override string ToString()
                => "Fail";
        }

        private sealed class MergeBehavior : IBehavior
        {
            public T Transform<T>(IDuplicateSectionBehaviorTransformation<T> transformation)
                => transformation.Merge();

            public bool Equals(IBehavior other)
                => other is MergeBehavior;

            public override int GetHashCode()
                => nameof(MergeBehavior).GetHashCode();

            public override string ToString()
                => "Merge";
        }
    }

    internal interface IDuplicateSectionBehaviorTransformation<out T>
    {
        T Fail();

        T Merge();
    }
}
