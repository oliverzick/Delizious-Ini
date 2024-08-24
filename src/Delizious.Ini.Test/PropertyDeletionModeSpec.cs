namespace Delizious.Ini.Test
{
    using System.Collections.Generic;

    [TestClass]
    public sealed class PropertyDeletionModeSpec
    {
        private static PropertyDeletionMode Fail => PropertyDeletionMode.Fail;
        private static PropertyDeletionMode Ignore => PropertyDeletionMode.Ignore;

        [DataTestMethod]
        [DynamicData(nameof(Provides_string_representation_test_cases), DynamicDataSourceType.Method)]
        public void Provides_string_representation(PropertyDeletionMode target, string expected)
        {
            var actual = target.ToString();

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Provides_string_representation_test_cases()
        {
            yield return new object[] { Fail, "Fail" };
            yield return new object[] { Ignore, "Ignore" };
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
        public void Properly_implements_equality_operator(PropertyDeletionMode left, PropertyDeletionMode right, bool expected)
        {
            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases),            DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases),       DynamicDataSourceType.Method)]
        public void Properly_implements_inequality_operator(PropertyDeletionMode left, PropertyDeletionMode right, bool inverse_expected)
        {
            var expected = !inverse_expected;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_test_cases),      DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_equals_method(PropertyDeletionMode target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(General_equals_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_test_cases),         DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_null_test_cases),    DynamicDataSourceType.Method)]
        public void Properly_implements_general_equals_method(PropertyDeletionMode target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_get_hash_code_method(PropertyDeletionMode target, PropertyDeletionMode other, bool expected)
        {
            var actual = target.GetHashCode() == other.GetHashCode();

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Equality_operator_test_cases()
        {
            yield return new object[] { null!, null!, true };
            yield return new object[] { null!, Fail, false };
            yield return new object[] { null!, Ignore, false };
        }

        public static IEnumerable<object[]> Equals_null_test_cases()
        {
            yield return new object[] { Fail, null!, false };
            yield return new object[] { Ignore, null!, false };
        }

        public static IEnumerable<object[]> Equals_test_cases()
        {
            yield return new object[] { Fail, Fail, true };
            yield return new object[] { Fail, Ignore, false };

            yield return new object[] { Ignore, Ignore, true };
            yield return new object[] { Ignore, Fail, false };
        }

        public static IEnumerable<object[]> General_equals_test_cases()
        {
            yield return new object[] { Fail, string.Empty, false };
            yield return new object[] { Ignore, string.Empty, false };
        }
    }
}
