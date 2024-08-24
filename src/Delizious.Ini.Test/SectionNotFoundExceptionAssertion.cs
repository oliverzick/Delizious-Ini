namespace Delizious.Ini.Test;

internal sealed record SectionNotFoundExceptionAssertion(SectionName SectionName)
{
    public static implicit operator SectionNotFoundExceptionAssertion(SectionNotFoundException exception)
        => new(exception.SectionName);
}
