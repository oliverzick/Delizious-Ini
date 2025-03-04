namespace Delizious.Ini.Test;

using System;
using System.Collections.Generic;

[TestClass]
public sealed class CommentSpec
{
    private static Comment Null => null!;
    private static Comment None => Comment.None;
    private static Comment SingleLineComment => "Single line comment";
    private static Comment MultiLineComment => $"Multi-line{Environment.NewLine}Comment";

    [TestMethod]
    public void Throws_argument_null_exception_on_creation_when_given_comment_is_null()
    {
        Assert.ThrowsException<ArgumentNullException>(() => Comment.Create(null));
    }

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(Comment target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return [None, string.Empty];
        yield return [SingleLineComment, "Single line comment"];
        yield return [MultiLineComment, $"Multi-line{Environment.NewLine}Comment"];
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(Comment left, Comment right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(Comment left, Comment right, bool unexpected)
    {
        var expected = !unexpected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(Comment target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(Comment target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(Comment target, Comment other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [Null, Null, true];
        yield return [Null, None, false];
        yield return [Null, SingleLineComment, false];
        yield return [Null, MultiLineComment, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [None, Null, false];
        yield return [SingleLineComment, Null, false];
        yield return [MultiLineComment, Null, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return [None, None, true];
        yield return [None, SingleLineComment, false];
        yield return [None, MultiLineComment, false];

        yield return [SingleLineComment, SingleLineComment, true];
        yield return [SingleLineComment, None, false];
        yield return [SingleLineComment, MultiLineComment, false];

        yield return [MultiLineComment, MultiLineComment, true];
        yield return [MultiLineComment, None, false];
        yield return [MultiLineComment, SingleLineComment, false];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [None, new(), false];
        yield return [SingleLineComment, new(), false];
        yield return [MultiLineComment, new(), false];
    }
}
