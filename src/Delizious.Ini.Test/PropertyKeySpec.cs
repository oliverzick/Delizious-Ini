namespace Delizious.Ini.Test;

using System;
using System.Collections.Generic;

[TestClass]
public sealed class PropertyKeySpec
{
    private const string Key = "Key";

    private static PropertyKey Null => null!;
    private static PropertyKey A => "A";
    private static PropertyKey B => "B";

    [TestMethod]
    public void Throws_argument_null_exception_on_creation_when_given_property_key_is_null()
    {
        Assert.ThrowsException<ArgumentNullException>(() => PropertyKey.Create(null));
    }

    [TestMethod]
    public void Throws_argument_exception_on_creation_when_given_property_key_is_empty()
    {
        Assert.ThrowsException<ArgumentException>(() => PropertyKey.Create(string.Empty));
    }

    [TestMethod]
    public void Throws_argument_exception_on_creation_when_given_property_key_consists_only_of_white_space_characters()
    {
        Assert.ThrowsException<ArgumentException>(() => PropertyKey.Create("   "));
    }

    [TestMethod]
    public void Represents_encapsulated_property_key()
    {
        const string expected = Key;

        PropertyKey target = expected;

        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Provides_hash_code_of_encapsulated_property_key()
    {
        var expected = Key.GetHashCode();

        PropertyKey target = Key;

        var actual = target.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(PropertyKey left, PropertyKey right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(PropertyKey left, PropertyKey right, bool unexpected)
    {
        var expected = !unexpected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(PropertyKey target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(PropertyKey target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(PropertyKey target, PropertyKey other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [Null, Null, true];
        yield return [Null, A, false];
        yield return [Null, B, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [A, Null, false];
        yield return [B, Null, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return [A, A, true];
        yield return [A, B, false];

        yield return [B, B, true];
        yield return [B, A, false];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [A, new(), false];
        yield return [B, new(), false];
    }
}
