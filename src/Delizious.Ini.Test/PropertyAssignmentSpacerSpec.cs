namespace Delizious.Ini.Test;

using System.Collections.Generic;

[TestClass]
public sealed class PropertyAssignmentSpacerSpec
{
    private static PropertyAssignmentSpacer Null => null!;
    private static PropertyAssignmentSpacer None => PropertyAssignmentSpacer.None;
    private static PropertyAssignmentSpacer Space => PropertyAssignmentSpacer.Space;

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(PropertyAssignmentSpacer target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return [None, string.Empty];
        yield return [Space, " "];
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(PropertyAssignmentSpacer left, PropertyAssignmentSpacer right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(PropertyAssignmentSpacer left, PropertyAssignmentSpacer right, bool inverse_expected)
    {
        var expected = !inverse_expected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(PropertyAssignmentSpacer target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(PropertyAssignmentSpacer target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(PropertyAssignmentSpacer target, PropertyAssignmentSpacer other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [Null, Null, true];
        yield return [Null, None, false];
        yield return [Null, Space, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [None, Null, false];
        yield return [Space, Null, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return [None, None, true];
        yield return [None, Space, false];

        yield return [Space, None, false];
        yield return [Space, Space, true];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [None, new(), false];
        yield return [Space, new(), false];
    }
}
