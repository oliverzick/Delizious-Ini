namespace Delizious.Ini.Test;

using System.Collections.Generic;

[TestClass]
public sealed class NewlineStringSpec
{
    private static NewlineString Null => null!;
    private static NewlineString Environment => NewlineString.Environment;
    private static NewlineString Unix => NewlineString.Unix;
    private static NewlineString Windows => NewlineString.Windows;

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(NewlineString target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return [Environment, System.Environment.NewLine];
        yield return [Unix, "\n"];
        yield return [Windows, "\r\n"];
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(NewlineString left, NewlineString right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(NewlineString left, NewlineString right, bool unexpected)
    {
        var expected = !unexpected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(NewlineString target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(NewlineString target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(NewlineString target, NewlineString other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [Null, Null, true];
        yield return [Null, Environment, false];
        yield return [Null, Unix, false];
        yield return [Null, Windows, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [Environment, Null, false];
        yield return [Unix, Null, false];
        yield return [Windows, Null, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        // Environment newline string is not tested against other predefined newline strings
        // to prevent coupling to specific underlying system
        yield return [Environment, Environment, true];

        yield return [Unix, Unix, true];
        yield return [Unix, Windows, false];

        yield return [Windows, Unix, false];
        yield return [Windows, Windows, true];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [Environment, new(), false];
        yield return [Unix, new(), false];
        yield return [Windows, new(), false];
    }
}
