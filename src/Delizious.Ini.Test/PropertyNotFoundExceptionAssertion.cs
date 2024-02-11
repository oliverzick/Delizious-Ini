namespace Delizious.Ini.Test
{
    internal sealed record PropertyNotFoundExceptionAssertion(PropertyKey PropertyKey)
    {
        public static implicit operator PropertyNotFoundExceptionAssertion(PropertyNotFoundException exception)
            => new(exception.PropertyKey);
    }
}
