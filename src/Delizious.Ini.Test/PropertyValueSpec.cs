namespace Delizious.Ini.Test;

using System;
using System.Collections.Generic;

[TestClass]
public sealed class PropertyValueSpec
{
    private const string Value = "Value";

    private static PropertyValue Null => null!;
    private static PropertyValue A => "A";
    private static PropertyValue B => "B";

    [TestMethod]
    public void Throws_argument_null_exception_on_creation_when_given_property_value_is_null()
    {
        Assert.ThrowsException<ArgumentNullException>(() => PropertyValue.Create(null));
    }

    [TestMethod]
    public void Represents_encapsulated_property_value()
    {
        const string expected = Value;

        PropertyValue target = expected;

        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Provides_hash_code_of_encapsulated_property_value()
    {
        var expected = Value.GetHashCode();

        PropertyValue target = Value;

        var actual = target.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(PropertyValue left, PropertyValue right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(PropertyValue left, PropertyValue right, bool inverse_expected)
    {
        var expected = !inverse_expected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(PropertyValue target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(PropertyValue target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(PropertyValue target, PropertyValue other, bool expected)
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
