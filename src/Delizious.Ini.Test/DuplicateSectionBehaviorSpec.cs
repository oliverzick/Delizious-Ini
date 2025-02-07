﻿namespace Delizious.Ini.Test;

using System.Collections.Generic;

[TestClass]
public sealed class DuplicateSectionBehaviorSpec
{
    private static DuplicateSectionBehavior Fail => DuplicateSectionBehavior.Fail;
    private static DuplicateSectionBehavior Merge => DuplicateSectionBehavior.Merge;

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(DuplicateSectionBehavior target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return [Fail, "Fail"];
        yield return [Merge, "Merge"];
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(DuplicateSectionBehavior left, DuplicateSectionBehavior right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(DuplicateSectionBehavior left, DuplicateSectionBehavior right, bool unexpected)
    {
        var expected = !unexpected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(DuplicateSectionBehavior target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(DuplicateSectionBehavior target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(DuplicateSectionBehavior target, DuplicateSectionBehavior other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [null!, null!, true];
        yield return [null!, Fail, false];
        yield return [null!, Merge, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [Fail, null!, false];
        yield return [Merge, null!, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return [Fail, Fail, true];
        yield return [Fail, Merge, false];

        yield return [Merge, Merge, true];
        yield return [Merge, Fail, false];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [Fail, new(), false];
        yield return [Merge, new(), false];
    }
}
