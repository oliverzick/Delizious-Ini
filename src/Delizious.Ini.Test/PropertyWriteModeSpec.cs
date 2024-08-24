namespace Delizious.Ini.Test;

using System.Collections.Generic;

[TestClass]
public sealed class PropertyWriteModeSpec
{
    private static PropertyWriteMode Create => PropertyWriteMode.Create;
    private static PropertyWriteMode Update => PropertyWriteMode.Update;

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(PropertyWriteMode target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return new object[] { Create, "Create" };
        yield return new object[] { Update, "Update" };
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(PropertyWriteMode left, PropertyWriteMode right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(PropertyWriteMode left, PropertyWriteMode right, bool inverse_expected)
    {
        var expected = !inverse_expected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(PropertyWriteMode target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(PropertyWriteMode target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(PropertyWriteMode target, PropertyWriteMode other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return new object[] { null!, null!, true };
        yield return new object[] { null!, Create, false };
        yield return new object[] { null!, Update, false };
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return new object[] { Create, null!, false };
        yield return new object[] { Update, null!, false };
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return new object[] { Create, Create, true };
        yield return new object[] { Create, Update, false };

        yield return new object[] { Update, Update, true };
        yield return new object[] { Update, Create, false };
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return new object[] { Create, string.Empty, false };
        yield return new object[] { Update, string.Empty, false };
    }
}
