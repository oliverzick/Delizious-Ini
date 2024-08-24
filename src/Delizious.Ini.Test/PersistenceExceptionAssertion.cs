namespace Delizious.Ini.Test;

using System;

internal sealed record PersistenceExceptionAssertion(Type InnerExceptionType)
{
    public static PersistenceExceptionAssertion Create<T>() where T : Exception
        => new(typeof(T));

    public static implicit operator PersistenceExceptionAssertion(PersistenceException exception)
        => new(exception.InnerException!.GetType());
}
