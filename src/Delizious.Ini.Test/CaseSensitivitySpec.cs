namespace Delizious.Ini.Test;

using System.Collections.Generic;

[TestClass]
public sealed class CaseSensitivitySpec
{
    private static CaseSensitivity Null => null!;
    private static CaseSensitivity CaseInsensitive => CaseSensitivity.CaseInsensitive;
    private static CaseSensitivity CaseSensitive => CaseSensitivity.CaseSensitive;

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(CaseSensitivity target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return [CaseInsensitive, "CaseInsensitive"];
        yield return [CaseSensitive, "CaseSensitive"];
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(CaseSensitivity left, CaseSensitivity right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(CaseSensitivity left, CaseSensitivity right, bool inverse_expected)
    {
        var expected = !inverse_expected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(CaseSensitivity target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(CaseSensitivity target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(CaseSensitivity target, CaseSensitivity other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [Null, Null, true];
        yield return [Null, CaseInsensitive, false];
        yield return [Null, CaseSensitive, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [CaseInsensitive, Null, false];
        yield return [CaseSensitive, Null, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return [CaseInsensitive, CaseInsensitive, true];
        yield return [CaseInsensitive, CaseSensitive, false];

        yield return [CaseSensitive, CaseSensitive, true];
        yield return [CaseSensitive, CaseInsensitive, false];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [CaseInsensitive, new(), false];
        yield return [CaseSensitive, new(), false];
    }
}
