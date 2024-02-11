namespace Delizious.Ini.Test
{
    using System;
    using System.Collections.Generic;

    [TestClass]
    public sealed class PropertyValueSpec
    {
        private const string Value = "Value";

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
        public void Properly_implements_equality_operator(PropertyValue left, PropertyValue right, bool expected)
        {
            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_inequality_operator(PropertyValue left, PropertyValue right, bool inverse_expected)
        {
            var expected = !inverse_expected;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Equality_operator_test_cases()
        {
            yield return new object[] { PropertyValues.Null, PropertyValues.Null, true };
            yield return new object[] { PropertyValues.A, PropertyValues.Null, false };
            yield return new object[] { PropertyValues.Null, PropertyValues.A, false };
            yield return new object[] { PropertyValues.A, PropertyValues.B, false };
            yield return new object[] { PropertyValues.B, PropertyValues.A, false };
            yield return new object[] { PropertyValues.A, PropertyValues.A, true };
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_method_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_equals_method(PropertyValue target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Equals_method_test_cases()
        {
            yield return new object[] { PropertyValues.A, PropertyValues.Null, false };
            yield return new object[] { PropertyValues.A, PropertyValues.B, false };
            yield return new object[] { PropertyValues.A, PropertyValues.A, true };
            yield return new object[] { PropertyValues.A, PropertyValues.AnotherA, true };
        }

        [DataTestMethod]
        [DynamicData(nameof(General_equals_method_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_method_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_general_equals_method(PropertyValue target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> General_equals_method_test_cases()
        {
            yield return new object[] { PropertyValues.A, string.Empty, false };
        }

        private static class PropertyValues
        {
            public static readonly PropertyValue Null = null!;
            public static readonly PropertyValue A = "A";
            public static readonly PropertyValue AnotherA = "A";
            public static readonly PropertyValue B = "B";
        }
    }
}
