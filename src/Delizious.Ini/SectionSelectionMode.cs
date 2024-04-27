namespace Delizious.Ini
{
    // ToDo: SectionSelectionMode - Implement value semantics
    internal sealed class SectionSelectionMode
    {
        private readonly IStrategy strategy;

        private SectionSelectionMode(IStrategy strategy)
        {
            this.strategy = strategy;
        }

        public static SectionSelectionMode FailIfMissing()
            => new SectionSelectionMode(new FailIfMissingStrategy());

        internal T Transform<T>(ISectionSelectionModeTransformation<T> transformation)
            => this.strategy.Transform(transformation);

        private interface IStrategy
        {
            T Transform<T>(ISectionSelectionModeTransformation<T> transformation);
        }

        private readonly struct FailIfMissingStrategy : IStrategy
        {
            public T Transform<T>(ISectionSelectionModeTransformation<T> transformation)
                => transformation.FailIfMissing();
        }
    }

    internal interface ISectionSelectionModeTransformation<T>
    {
        T FailIfMissing();
    }
}
