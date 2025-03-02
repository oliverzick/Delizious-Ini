namespace Delizious.Ini.Test;

using System;
using System.Collections.Generic;

[TestClass]
public sealed class CommentReadModeSpec
{
    private static CommentReadMode Fail => CommentReadMode.Fail;
    private static CommentReadMode Fallback => CommentReadMode.Fallback;
    private static CommentReadMode FallbackCustom => CommentReadMode.CustomFallback("Fallback");
    private static CommentReadMode FallbackCustomDefault => CommentReadMode.CustomFallback(string.Empty);

    [TestMethod]
    public void Throws_argument_null_exception_when_fallback_comment_is_null()
    {
        Assert.ThrowsException<ArgumentNullException>(() => CommentReadMode.CustomFallback(null));
    }

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(CommentReadMode target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return [Fail, "Fail"];
        yield return [Fallback, "Fallback"];
        yield return [FallbackCustom, "Fallback"];
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(CommentReadMode left, CommentReadMode right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(CommentReadMode left, CommentReadMode right, bool unexpected)
    {
        var expected = !unexpected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(CommentReadMode target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(CommentReadMode target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(CommentReadMode target, CommentReadMode other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [null!, null!, true];
        yield return [null!, Fail, false];
        yield return [null!, Fallback, false];
        yield return [null!, FallbackCustom, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [Fail, null!, false];
        yield return [Fallback, null!, false];
        yield return [FallbackCustom, null!, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return [Fail, Fail, true];
        yield return [Fail, Fallback, false];
        yield return [Fail, FallbackCustom, false];

        yield return [Fallback, Fallback, true];
        yield return [Fallback, FallbackCustom, false];
        yield return [Fallback, Fail, false];

        yield return [FallbackCustom, FallbackCustom, true];
        yield return [FallbackCustom, Fallback, false];
        yield return [FallbackCustom, Fail, false];

        // Specifies that default fallback is the same as custom fallback with empty string
        yield return [Fallback, FallbackCustomDefault, true];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [Fail, new(), false];
        yield return [Fallback, new(), false];
        yield return [FallbackCustom, new(), false];
    }
}
