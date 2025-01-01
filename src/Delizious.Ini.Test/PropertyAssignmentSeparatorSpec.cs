namespace Delizious.Ini.Test;

using System.Collections.Generic;

[TestClass]
public sealed class PropertyAssignmentSeparatorSpec
{
    private static PropertyAssignmentSeparator Null => null!;
    private static PropertyAssignmentSeparator Default => PropertyAssignmentSeparator.Default;
    private static PropertyAssignmentSeparator EqualitySign => '=';
    private static PropertyAssignmentSeparator Colon => ':';

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(PropertyAssignmentSeparator target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return [Default, "="];
        yield return [EqualitySign, "="];
        yield return [Colon, ":"];
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(PropertyAssignmentSeparator left, PropertyAssignmentSeparator right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(PropertyAssignmentSeparator left, PropertyAssignmentSeparator right, bool inverse_expected)
    {
        var expected = !inverse_expected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(PropertyAssignmentSeparator target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(PropertyAssignmentSeparator target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(PropertyAssignmentSeparator target, PropertyAssignmentSeparator other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [Null, Null, true];
        yield return [Null, Default, false];
        yield return [Null, EqualitySign, false];
        yield return [Null, Colon, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [Default, Null, false];
        yield return [EqualitySign, Null, false];
        yield return [Colon, Null, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return [Default, Default, true];
        yield return [Default, EqualitySign, true];
        yield return [Default, Colon, false];

        yield return [EqualitySign, Default, true];
        yield return [EqualitySign, EqualitySign, true];
        yield return [EqualitySign, Colon, false];

        yield return [Colon, Default, false];
        yield return [Colon, EqualitySign, false];
        yield return [Colon, Colon, true];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [Default, new(), false];
        yield return [EqualitySign, new(), false];
        yield return [Colon, new(), false];
    }
}
