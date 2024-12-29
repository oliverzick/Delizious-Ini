namespace Delizious.Ini.Test;

using System.Collections.Generic;

[TestClass]
public sealed class InvalidLineBehaviorSpec
{
    private static InvalidLineBehavior Fail => InvalidLineBehavior.Fail;
    private static InvalidLineBehavior Ignore => InvalidLineBehavior.Ignore;

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(InvalidLineBehavior target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return [Fail, "Fail"];
        yield return [Ignore, "Ignore"];
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(InvalidLineBehavior left, InvalidLineBehavior right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(InvalidLineBehavior left, InvalidLineBehavior right, bool inverse_expected)
    {
        var expected = !inverse_expected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(InvalidLineBehavior target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(InvalidLineBehavior target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(InvalidLineBehavior target, InvalidLineBehavior other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [null!, null!, true];
        yield return [null!, Fail, false];
        yield return [null!, Ignore, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [Fail, null!, false];
        yield return [Ignore, null!, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return [Fail, Fail, true];
        yield return [Fail, Ignore, false];

        yield return [Ignore, Ignore, true];
        yield return [Ignore, Fail, false];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [Fail, string.Empty, false];
        yield return [Ignore, string.Empty, false];
    }
}