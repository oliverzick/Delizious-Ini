﻿namespace Delizious.Ini.Test
{
    using System.Collections.Generic;

    [TestClass]
    public sealed class PropertyEnumerationModeSpec
    {
        [DataTestMethod]
        [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
        public void Provides_string_representation(PropertyEnumerationMode target, string expected)
        {
            var actual = target.ToString();

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Provides_string_representation_test_cases()
        {
            yield return new object[] { PropertyEnumerationMode.Fail(), "Fail" };
            yield return new object[] { PropertyEnumerationMode.Fallback(), "Fallback" };
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_equality_operator(PropertyEnumerationMode left, PropertyEnumerationMode right, bool expected)
        {
            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_inequality_operator(PropertyEnumerationMode left, PropertyEnumerationMode right, bool inverse_expected)
        {
            var expected = !inverse_expected;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_equals_method(PropertyEnumerationMode target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_general_equals_method(PropertyEnumerationMode target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_get_hash_code_method(PropertyEnumerationMode target, PropertyEnumerationMode other, bool expected)
        {
            var actual = target.GetHashCode() == other.GetHashCode();

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Equality_operator_test_cases()
        {
            yield return new object[] { null!, null!, true };
            yield return new object[] { null!, PropertyEnumerationMode.Fail(), false };
            yield return new object[] { null!, PropertyEnumerationMode.Fallback(), false };
        }
        public static IEnumerable<object[]> Equals_null_test_cases()
        {
            yield return new object[] { PropertyEnumerationMode.Fail(), null!, false };
            yield return new object[] { PropertyEnumerationMode.Fallback(), null!, false };
        }

        public static IEnumerable<object[]> Equals_test_cases()
        {
            yield return new object[] { PropertyEnumerationMode.Fail(), PropertyEnumerationMode.Fail(), true };
            yield return new object[] { PropertyEnumerationMode.Fail(), PropertyEnumerationMode.Fallback(), false };
            yield return new object[] { PropertyEnumerationMode.Fallback(), PropertyEnumerationMode.Fallback(), true };
            yield return new object[] { PropertyEnumerationMode.Fallback(), PropertyEnumerationMode.Fail(), false };
        }

        public static IEnumerable<object[]> General_equals_test_cases()
        {
            yield return new object[] { PropertyEnumerationMode.Fail(), string.Empty, false };
            yield return new object[] { PropertyEnumerationMode.Fallback(), string.Empty, false };
        }
    }
}