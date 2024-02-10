namespace Delizious.Ini.Test
{
    using System;
    using System.Collections.Generic;

    [TestClass]
    public sealed class PropertyKeySpec
    {
        private const string Key = "Key";

        [TestMethod]
        public void Throws_exception_on_creation_when_given_property_key_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => PropertyKey.Create(null));
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
        public void Properly_implements_equality_operator(PropertyKey left, PropertyKey right, bool expected)
        {
            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_inequality_operator(PropertyKey left, PropertyKey right, bool inverse_expected)
        {
            var expected = !inverse_expected;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Equality_operator_test_cases()
        {
            yield return new object[] { PropertyKeys.Null, PropertyKeys.Null, true };
            yield return new object[] { PropertyKeys.A, PropertyKeys.Null, false };
            yield return new object[] { PropertyKeys.Null, PropertyKeys.A, false };
            yield return new object[] { PropertyKeys.A, PropertyKeys.B, false };
            yield return new object[] { PropertyKeys.B, PropertyKeys.A, false };
            yield return new object[] { PropertyKeys.A, PropertyKeys.A, true };
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_method_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_equals_method(PropertyKey target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Equals_method_test_cases()
        {
            yield return new object[] { PropertyKeys.A, PropertyKeys.Null, false };
            yield return new object[] { PropertyKeys.A, PropertyKeys.B, false };
            yield return new object[] { PropertyKeys.A, PropertyKeys.A, true };
            yield return new object[] { PropertyKeys.A, PropertyKeys.AnotherA, true };
        }

        [DataTestMethod]
        [DynamicData(nameof(General_equals_method_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_method_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_general_equals_method(PropertyKey target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> General_equals_method_test_cases()
        {
            yield return new object[] { PropertyKeys.A, string.Empty, false };
        }

        private static class PropertyKeys
        {
            public static readonly PropertyKey Null = null!;
            public static readonly PropertyKey A = "A";
            public static readonly PropertyKey AnotherA = "A";
            public static readonly PropertyKey B = "B";
        }
    }
}
