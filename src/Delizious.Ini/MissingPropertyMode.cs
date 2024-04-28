﻿namespace Delizious.Ini
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

        internal T Transform<T>(IPropertyModeTransformation<T> transformation)
            => this.strategy.Transform(transformation);

        private interface IStrategy
        {
            T Transform<T>(IPropertyModeTransformation<T> transformation);
        }

        private readonly struct FailStrategy : IStrategy
        {
            public T Transform<T>(IPropertyModeTransformation<T> transformation)
                => transformation.Fail();
        }
    }

    internal interface IPropertyModeTransformation<T>
    {
        T Fail();
    }
}
