namespace Delizious.Ini.Test;

using System;
using System.Collections.Generic;

[TestClass]
public sealed class SectionNameRegexSpec
{
    private static SectionNameRegex Null => null!;
    private static SectionNameRegex Default => SectionNameRegex.Default;
    private static SectionNameRegex Custom => SectionNameRegex.Create(@"\w+");

    [TestMethod]
    public void Throws_argument_null_exception_on_creation_when_given_pattern_is_null()
    {
        Assert.ThrowsException<ArgumentNullException>(() => SectionNameRegex.Create(null));
    }

    [TestMethod]
    public void Throws_argument_exception_on_creation_when_given_pattern_is_empty()
    {
        Assert.ThrowsException<ArgumentException>(() => SectionNameRegex.Create(string.Empty));
    }

    [TestMethod]
    public void Throws_argument_exception_on_creation_when_given_pattern_is_invalid()
    {
        Assert.ThrowsException<ArgumentException>(() => SectionNameRegex.Create(@"\"));
    }

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(SectionNameRegex target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return [Default, @"[\p{L}\p{M}\p{N}\p{P}\p{S}\p{Zs}]+"];
        yield return [Custom, @"\w+"];
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(SectionNameRegex left, SectionNameRegex right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(SectionNameRegex left, SectionNameRegex right, bool unexpected)
    {
        var expected = !unexpected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(SectionNameRegex target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(SectionNameRegex target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(SectionNameRegex target, SectionNameRegex other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [Null, Null, true];
        yield return [Null, Default, false];
        yield return [Null, Custom, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [Default, Null, false];
        yield return [Custom, Null, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return [Default, Default, true];
        yield return [Default, Custom, false];

        yield return [Custom, Default, false];
        yield return [Custom, Custom, true];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [Default, new(), false];
        yield return [Custom, new(), false];
    }
}
