namespace Delizious.Ini.Test;

using System.Collections.Generic;

[TestClass]
public sealed class SectionBeginningDelimiterSpec
{
    private static SectionBeginningDelimiter Null => null!;
    private static SectionBeginningDelimiter Default => SectionBeginningDelimiter.Default;
    private static SectionBeginningDelimiter Bracket => '[';
    private static SectionBeginningDelimiter Parentheses => '(';

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(SectionBeginningDelimiter target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return [Default, "["];
        yield return [Bracket, "["];
        yield return [Parentheses, "("];
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(SectionBeginningDelimiter left, SectionBeginningDelimiter right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(SectionBeginningDelimiter left, SectionBeginningDelimiter right, bool inverse_expected)
    {
        var expected = !inverse_expected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(SectionBeginningDelimiter target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(SectionBeginningDelimiter target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(SectionBeginningDelimiter target, SectionBeginningDelimiter other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [Null, Null, true];
        yield return [Null, Default, false];
        yield return [Null, Bracket, false];
        yield return [Null, Parentheses, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [Default, Null, false];
        yield return [Bracket, Null, false];
        yield return [Parentheses, Null, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return [Default, Default, true];
        yield return [Default, Bracket, true];
        yield return [Default, Parentheses, false];

        yield return [Bracket, Default, true];
        yield return [Bracket, Bracket, true];
        yield return [Bracket, Parentheses, false];

        yield return [Parentheses, Default, false];
        yield return [Parentheses, Bracket, false];
        yield return [Parentheses, Parentheses, true];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [Default, new(), false];
        yield return [Bracket, new(), false];
        yield return [Parentheses, new(), false];
    }
}
