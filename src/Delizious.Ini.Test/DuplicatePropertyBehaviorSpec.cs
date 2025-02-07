namespace Delizious.Ini.Test;

using System.Collections.Generic;

[TestClass]
public sealed class DuplicatePropertyBehaviorSpec
{
    private static DuplicatePropertyBehavior Fail => DuplicatePropertyBehavior.Fail;
    private static DuplicatePropertyBehavior Ignore => DuplicatePropertyBehavior.Ignore;
    private static DuplicatePropertyBehavior Override => DuplicatePropertyBehavior.Override;

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(DuplicatePropertyBehavior target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return [Fail, "Fail"];
        yield return [Ignore, "Ignore"];
        yield return [Override, "Override"];
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(DuplicatePropertyBehavior left, DuplicatePropertyBehavior right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(DuplicatePropertyBehavior left, DuplicatePropertyBehavior right, bool unexpected)
    {
        var expected = !unexpected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(DuplicatePropertyBehavior target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(DuplicatePropertyBehavior target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(DuplicatePropertyBehavior target, DuplicatePropertyBehavior other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [null!, null!, true];
        yield return [null!, Fail, false];
        yield return [null!, Ignore, false];
        yield return [null!, Override, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [Fail, null!, false];
        yield return [Ignore, null!, false];
        yield return [Override, null!, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return [Fail, Fail, true];
        yield return [Fail, Ignore, false];
        yield return [Fail, Override, false];

        yield return [Ignore, Fail, false];
        yield return [Ignore, Ignore, true];
        yield return [Ignore, Override, false];

        yield return [Override, Fail, false];
        yield return [Override, Ignore, false];
        yield return [Override, Override, true];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [Fail, new(), false];
        yield return [Ignore, new(), false];
        yield return [Override, new(), false];
    }
}
