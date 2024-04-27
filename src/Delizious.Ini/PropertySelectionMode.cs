namespace Delizious.Ini
{
    // ToDo: PropertySelectionMode - Implement value semantics
    internal sealed class PropertySelectionMode
    {
        private readonly IStrategy strategy;

        private PropertySelectionMode(IStrategy strategy)
        {
            this.strategy = strategy;
        }

        public static PropertySelectionMode FailIfMissing()
            => new PropertySelectionMode(new FailIfMissingStrategy());

        internal T Transform<T>(IPropertyModeTransformation<T> transformation)
            => this.strategy.Transform(transformation);

        private interface IStrategy
        {
            T Transform<T>(IPropertyModeTransformation<T> transformation);
        }

        private readonly struct FailIfMissingStrategy : IStrategy
        {
            public T Transform<T>(IPropertyModeTransformation<T> transformation)
                => transformation.FailIfMissing();
        }
    }

    internal interface IPropertyModeTransformation<T>
    {
        T FailIfMissing();
    }
}
