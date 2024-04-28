namespace Delizious.Ini
{
    // ToDo: MissingSectionMode - Implement value semantics
    internal sealed class MissingSectionMode
    {
        private readonly IStrategy strategy;

        private MissingSectionMode(IStrategy strategy)
        {
            this.strategy = strategy;
        }

        public static MissingSectionMode Fail()
            => new MissingSectionMode(new FailStrategy());

        internal T Transform<T>(IMissingSectionModeTransformation<T> transformation)
            => this.strategy.Transform(transformation);

        private interface IStrategy
        {
            T Transform<T>(IMissingSectionModeTransformation<T> transformation);
        }

        private readonly struct FailStrategy : IStrategy
        {
            public T Transform<T>(IMissingSectionModeTransformation<T> transformation)
                => transformation.Fail();
        }
    }

    internal interface IMissingSectionModeTransformation<T>
    {
        T Fail();
    }
}
