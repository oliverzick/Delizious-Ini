namespace Delizious.Ini.Test;

using System;
using System.Collections.Generic;

[TestClass]
public sealed class CommentStringSpec
{
    private static CommentString Null => null!;
    private static CommentString Default => CommentString.Default;
    private static CommentString Semicolon => ";";
    private static CommentString Hashtag => "#";

    [TestMethod]
    public void Throws_argument_null_exception_on_creation_when_given_comment_string_is_null()
    {
        Assert.ThrowsException<ArgumentNullException>(() => CommentString.Create(null));
    }

    [TestMethod]
    public void Throws_argument_exception_on_creation_when_given_comment_string_is_empty()
    {
        Assert.ThrowsException<ArgumentException>(() => CommentString.Create(string.Empty));
    }

    [TestMethod]
    public void Throws_argument_exception_on_creation_when_given_comment_string_consists_only_of_white_space_characters()
    {
        Assert.ThrowsException<ArgumentException>(() => CommentString.Create("   "));
    }

    [DataTestMethod]
    [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
    public void Provides_string_representation(CommentString target, string expected)
    {
        var actual = target.ToString();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Provides_string_representation_test_cases()
    {
        yield return [Default, ";"];
        yield return [Semicolon, ";"];
        yield return [Hashtag, "#"];
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_equality_operator(CommentString left, CommentString right, bool expected)
    {
        var actual = left == right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
    public void Properly_implements_inequality_operator(CommentString left, CommentString right, bool unexpected)
    {
        var expected = !unexpected;

        var actual = left != right;

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_equals_method(CommentString target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
    [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
    public void Properly_implements_general_equals_method(CommentString target, object other, bool expected)
    {
        var actual = target.Equals(other);

        Assert.AreEqual(expected, actual);
    }

    [DataTestMethod]
    [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
    public void Properly_implements_get_hash_code_method(CommentString target, CommentString other, bool expected)
    {
        var actual = target.GetHashCode() == other.GetHashCode();

        Assert.AreEqual(expected, actual);
    }

    public static IEnumerable<object[]> Equality_operator_test_cases()
    {
        yield return [Null, Null, true];
        yield return [Null, Default, false];
        yield return [Null, Semicolon, false];
        yield return [Null, Hashtag, false];
    }

    public static IEnumerable<object[]> Equals_null_test_cases()
    {
        yield return [Default, Null, false];
        yield return [Semicolon, Null, false];
        yield return [Hashtag, Null, false];
    }

    public static IEnumerable<object[]> Equals_test_cases()
    {
        yield return [Default, Default, true];
        yield return [Default, Semicolon, true];
        yield return [Default, Hashtag, false];

        yield return [Semicolon, Default, true];
        yield return [Semicolon, Semicolon, true];
        yield return [Semicolon, Hashtag, false];

        yield return [Hashtag, Default, false];
        yield return [Hashtag, Semicolon, false];
        yield return [Hashtag, Hashtag, true];
    }

    public static IEnumerable<object[]> General_equals_test_cases()
    {
        yield return [Default, new(), false];
        yield return [Semicolon, new(), false];
        yield return [Hashtag, new(), false];
    }
}
