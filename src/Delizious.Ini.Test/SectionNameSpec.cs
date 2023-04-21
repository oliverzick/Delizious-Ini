namespace Delizious.Ini.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System;
    using System.Collections.Generic;

    [TestClass]
    public sealed class SectionNameSpec
    {
        private const string Section = "Section";

        [TestMethod]
        public void Throws_exception_when_encapsulated_section_name_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SectionName.Create(null));
        }

        [TestMethod]
        public void Represents_encapsulated_section_name()
        {
            const string expected = Section;

            SectionName target = expected;

            var actual = target.ToString();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Provides_hash_code_of_encapsulated_section_name()
        {
            var expected = Section.GetHashCode();

            SectionName target = Section;

            var actual = target.GetHashCode();

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_equality_operator(SectionName left, SectionName right, bool expected)
        {
            var actual = left == right;

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Equality_operator_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_inequality_operator(SectionName left, SectionName right, bool inverse_expected)
        {
            var expected = !inverse_expected;

            var actual = left != right;

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Equality_operator_test_cases()
        {
            yield return new object[] { SectionNames.Null, SectionNames.Null, true };
            yield return new object[] { SectionNames.A, SectionNames.Null, false };
            yield return new object[] { SectionNames.Null, SectionNames.A, false };
            yield return new object[] { SectionNames.A, SectionNames.B, false };
            yield return new object[] { SectionNames.B, SectionNames.A, false };
            yield return new object[] { SectionNames.A, SectionNames.A, true };
        }

        [DataTestMethod]
        [DynamicData(nameof(Equals_method_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_equals_method(SectionName target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Equals_method_test_cases()
        {
            yield return new object[] { SectionNames.A, SectionNames.Null, false };
            yield return new object[] { SectionNames.A, SectionNames.B, false };
            yield return new object[] { SectionNames.A, SectionNames.A, true };
            yield return new object[] { SectionNames.A, SectionNames.AnotherA, true };
        }

        [DataTestMethod]
        [DynamicData(nameof(General_equals_method_test_cases), DynamicDataSourceType.Method)]
        [DynamicData(nameof(Equals_method_test_cases), DynamicDataSourceType.Method)]
        public void Properly_implements_general_equals_method(SectionName target, object other, bool expected)
        {
            var actual = target.Equals(other);

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> General_equals_method_test_cases()
        {
            yield return new object[] { SectionNames.A, string.Empty, false };
        }

        private static class SectionNames
        {
            public static readonly SectionName Null = null!;
            public static readonly SectionName A = "A";
            public static readonly SectionName AnotherA = "A";
            public static readonly SectionName B = "B";
        }
    }
}
