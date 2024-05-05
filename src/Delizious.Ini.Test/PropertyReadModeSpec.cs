namespace Delizious.Ini.Test
{
    using System;
    using System.Collections.Generic;

    [TestClass]
    public sealed class PropertyReadModeSpec
    {
        private static PropertyReadMode Fail => PropertyReadMode.Fail();
        private static PropertyReadMode Fallback => PropertyReadMode.Fallback();
        private static PropertyReadMode FallbackCustom => PropertyReadMode.Fallback("Fallback");
        private static PropertyReadMode FallbackCustomDefault => PropertyReadMode.Fallback(string.Empty);

        [TestMethod]
        public void Throws_argument_null_exception_when_fallback_property_value_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => PropertyReadMode.Fallback(null));
        }

        [DataTestMethod]
        [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
        public void Provides_string_representation(PropertyReadMode target, string expected)
        {
            var actual = target.ToString();

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Provides_string_representation_test_cases()
        {
            yield return new object[] { Fail, "Fail" };
            yield return new object[] { Fallback, "Fallback" };
            yield return new object[] { FallbackCustom, "Fallback" };
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_equality_operator(PropertyReadMode left, PropertyReadMode right, bool expected)
        {
            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_inequality_operator(PropertyReadMode left, PropertyReadMode right, bool inverse_expected)
        {
            var expected = !inverse_expected;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_equals_method(PropertyReadMode target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_general_equals_method(PropertyReadMode target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_get_hash_code_method(PropertyReadMode target, PropertyReadMode other, bool expected)
        {
            var actual = target.GetHashCode() == other.GetHashCode();

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Equality_operator_test_cases()
        {
            yield return new object[] { null!, null!, true };
            yield return new object[] { null!, Fail, false };
            yield return new object[] { null!, Fallback, false };
            yield return new object[] { null!, FallbackCustom, false };
        }

        public static IEnumerable<object[]> Equals_null_test_cases()
        {
            yield return new object[] { Fail, null!, false };
            yield return new object[] { Fallback, null!, false };
            yield return new object[] { FallbackCustom, null!, false };
        }

        public static IEnumerable<object[]> Equals_test_cases()
        {
            yield return new object[] { Fail, Fail, true };
            yield return new object[] { Fail, Fallback, false };
            yield return new object[] { Fail, FallbackCustom, false };

            yield return new object[] { Fallback, Fallback, true };
            yield return new object[] { Fallback, FallbackCustom, false };
            yield return new object[] { Fallback, Fail, false };

            yield return new object[] { FallbackCustom, FallbackCustom, true };
            yield return new object[] { FallbackCustom, Fallback, false };
            yield return new object[] { FallbackCustom, Fail, false };

            // Specifies that default fallback is the same as custom fallback with empty string
            yield return new object[] { Fallback, FallbackCustomDefault, true };
        }

        public static IEnumerable<object[]> General_equals_test_cases()
        {
            yield return new object[] { Fail, string.Empty, false };
            yield return new object[] { Fallback, string.Empty, false };
            yield return new object[] { FallbackCustom, string.Empty, false };
        }
    }
}
