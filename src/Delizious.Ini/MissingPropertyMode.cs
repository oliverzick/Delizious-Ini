namespace Delizious.Ini
{
    // ToDo: MissingPropertyMode - Implement value semantics
    internal sealed class MissingPropertyMode
    {
        private readonly IStrategy strategy;

        private MissingPropertyMode(IStrategy strategy)
        {
            this.strategy = strategy;
        }

        public static MissingPropertyMode Fail()
            => new MissingPropertyMode(new FailStrategy());

        internal T Transform<T>(IMissingPropertyModeTransformation<T> transformation)
            => this.strategy.Transform(transformation);

        private interface IStrategy
        {
            T Transform<T>(IMissingPropertyModeTransformation<T> transformation);
        }

        private readonly struct FailStrategy : IStrategy
        {
            public T Transform<T>(IMissingPropertyModeTransformation<T> transformation)
                => transformation.Fail();
        }
    }

    internal interface IMissingPropertyModeTransformation<T>
    {
        T Fail();
    }
}
