﻿namespace Delizious.Ini.Test
{
    using System;
    using System.Collections.Generic;

    [TestClass]
    public sealed class SectionNameSpec
    {
        private const string Section = "Section";

        private static SectionName Null => null!;
        private static SectionName A => "A";
        private static SectionName AnotherA => "A";
        private static SectionName B => "B";

        [TestMethod]
        public void Throws_argument_null_exception_on_creation_when_given_section_name_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => SectionName.Create(null));
        }

        [TestMethod]
        public void Throws_argument_exception_on_creation_when_given_section_name_is_empty()
        {
            Assert.ThrowsException<ArgumentException>(() => SectionName.Create(string.Empty));
        }

        [TestMethod]
        public void Throws_argument_exception_on_creation_when_given_section_name_consists_only_of_white_space_characters()
        {
            Assert.ThrowsException<ArgumentException>(() => SectionName.Create("   "));
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
            yield return new object[] { Null, Null, true };
            yield return new object[] { A, Null, false };
            yield return new object[] { Null, A, false };
            yield return new object[] { A, B, false };
            yield return new object[] { B, A, false };
            yield return new object[] { A, A, true };
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
            yield return new object[] { A, Null, false };
            yield return new object[] { A, B, false };
            yield return new object[] { A, A, true };
            yield return new object[] { A, AnotherA, true };
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
            yield return new object[] { A, string.Empty, false };
        }
    }
}
