namespace Delizious.Ini.Test
{
    using System;

    internal sealed record SerializationExceptionAssertion(Type InnerExceptionType)
    {
        public static SerializationExceptionAssertion Create<T>() where T : Exception
            => new(typeof(T));

        public static implicit operator SerializationExceptionAssertion(SerializationException exception)
            => new(exception.InnerException!.GetType());
    }
}
