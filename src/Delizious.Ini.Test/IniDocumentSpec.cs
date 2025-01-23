namespace Delizious.Ini.Test;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;

[TestClass]
public sealed class IniDocumentSpec
{
    private static IniDocumentConfiguration DefaultConfiguration => IniDocumentConfiguration.Default;

    private static SectionName DummySectionName => "Dummy";
    private static PropertyKey DummyPropertyKey => "Dummy";
    private static PropertyValue DummyPropertyValue => "Dummy";

    private static SectionName NonexistentSectionName => "NonexistentSection";
    private static PropertyKey NonexistentPropertyKey => "NonexistentProperty";

    private static SectionName DefaultSectionName => "Section";
    private static PropertyKey DefaultPropertyKey => "Property";
    private static PropertyValue DefaultPropertyValue => "Value";

    private static PropertyValue EmptyPropertyValue => string.Empty;

    [TestClass]
    public sealed class CreateEmpty
    {
        [TestMethod]
        public void Throws_argument_null_exception_when_configuration_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => IniDocument.CreateEmpty(null));
        }

        [TestMethod]
        public void Creates_empty_ini_document()
        {
            var target = IniDocument.CreateEmpty(DefaultConfiguration);

            Assert.IsFalse(target.EnumerateSections().Any());
        }
    }

    [TestClass]
    public sealed class LoadFrom
    {
        [TestMethod]
        public void Throws_argument_null_exception_when_text_reader_is_null()
        {
            Assert.ThrowsException<ArgumentNullException>(() => IniDocument.LoadFrom(null, DefaultConfiguration));
        }

        [TestMethod]
        public void Throws_argument_null_exception_when_configuration_is_null()
        {
            using var textReader = new StringReader(string.Empty);

            Assert.ThrowsException<ArgumentNullException>(() => IniDocument.LoadFrom(textReader, null));
        }

        [TestMethod]
        public void Throws_persistence_exception_containing_object_disposed_exception_when_text_reader_is_already_disposed()
        {
            var expected = PersistenceExceptionAssertion.Create<ObjectDisposedException>();
            using var textReader = new StringReader(string.Empty);
            textReader.Close();

            var actual = Assert.ThrowsException<PersistenceException>(() => IniDocument.LoadFrom(textReader, DefaultConfiguration));

            Assert.AreEqual(expected, actual);
        }

        [TestClass]
        public sealed class With_configured_duplicate_property_behavior
        {
            private const string Ini = """
                                       [Section]
                                       Property=First occurrence
                                       Property=Second occurrence
                                       Property=Last occurrence

                                       [AnotherSection]
                                       Property=Another occurrence
                                       """;

            private static IniDocumentConfiguration BaseConfiguration => IniDocumentConfiguration.Strict;

            [TestClass]
            public sealed class When_fail_behavior
            {
                private static IniDocumentConfiguration Configuration => BaseConfiguration.WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Fail);

                [TestMethod]
                public void Throws_persistence_exception_for_duplicated_property()
                {
                    using var reader = new StringReader(Ini);

                    Assert.ThrowsException<PersistenceException>(() => IniDocument.LoadFrom(reader, Configuration));
                }
            }

            [TestClass]
            public sealed class When_ignore_behavior
            {
                private const string ExpectedIni = """
                                                   [Section]
                                                   Property=First occurrence

                                                   [AnotherSection]
                                                   Property=Another occurrence

                                                   """;

                private static IniDocumentConfiguration Configuration => BaseConfiguration.WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Ignore);

                [TestMethod]
                public void Ignores_subsequent_occurrences_of_a_duplicate_property_by_using_the_first_occurrence_of_such_a_property()
                    => Load_and_save(Ini, Configuration, ExpectedIni);
            }

            [TestClass]
            public sealed class When_override_behavior
            {
                private const string ExpectedIni = """
                                                   [Section]
                                                   Property=Last occurrence

                                                   [AnotherSection]
                                                   Property=Another occurrence

                                                   """;

                private static IniDocumentConfiguration Configuration => BaseConfiguration.WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Override);

                [TestMethod]
                public void Overrides_previous_occurrences_of_a_duplicate_property_by_using_the_last_occurrence_of_such_a_property()
                    => Load_and_save(Ini, Configuration, ExpectedIni);
            }
        }

        [TestClass]
        public sealed class With_configured_duplicate_section_behavior_and_duplicate_property_behavior
        {
            private const string Ini = """
                                       [Section]
                                       Property=First occurrence

                                       [Section]
                                       Property=Second occurrence

                                       [Section]
                                       Property=Last occurrence

                                       [AnotherSection]
                                       Property=Another occurrence
                                       """;

            [TestClass]
            public sealed class When_merge_duplicate_section_and_ignore_duplicate_property
            {
                private const string ExpectedIni = """
                                                   [Section]
                                                   Property=First occurrence

                                                   [AnotherSection]
                                                   Property=Another occurrence

                                                   """;

                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Strict
                                                                                                 .WithDuplicateSectionBehavior(DuplicateSectionBehavior.Merge)
                                                                                                 .WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Ignore);

                [TestMethod]
                public void Ignores_subsequent_occurrences_of_a_duplicate_property_by_using_the_first_occurrence_of_such_a_property()
                    => Load_and_save(Ini, Configuration, ExpectedIni);
            }

            [TestClass]
            public sealed class When_merge_duplicate_section_and_override_duplicate_property
            {
                private const string ExpectedIni = """
                                                   [Section]
                                                   Property=Last occurrence

                                                   [AnotherSection]
                                                   Property=Another occurrence

                                                   """;

                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Strict
                                                                                                 .WithDuplicateSectionBehavior(DuplicateSectionBehavior.Merge)
                                                                                                 .WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Override);

                [TestMethod]
                public void Overrides_previous_occurrences_of_a_duplicate_property_by_using_the_last_occurrence_of_such_a_property()
                    => Load_and_save(Ini, Configuration, ExpectedIni);
            }
        }

        [TestClass]
        public sealed class With_configured_duplicate_section_behavior
        {
            private const string Ini = """
                                       [Section]
                                       PropertyA=Value A

                                       [AnotherSection]
                                       AnotherProperty=Another value

                                       [Section]
                                       PropertyB=Value B
                                       """;

            [TestClass]
            public sealed class When_fail_behavior
            {
                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Strict
                                                                                                 .WithDuplicateSectionBehavior(DuplicateSectionBehavior.Fail);

                [TestMethod]
                public void Throws_persistence_exception_for_duplicated_section()
                {
                    using var reader = new StringReader(Ini);

                    Assert.ThrowsException<PersistenceException>(() => IniDocument.LoadFrom(reader, Configuration));
                }
            }

            [TestClass]
            public sealed class When_merge_behavior
            {
                private const string ExpectedIni = """
                                                   [Section]
                                                   PropertyA=Value A
                                                   PropertyB=Value B

                                                   [AnotherSection]
                                                   AnotherProperty=Another value

                                                   """;

                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Strict
                                                                                                 .WithDuplicateSectionBehavior(DuplicateSectionBehavior.Merge);

                [TestMethod]
                public void Merges_duplicated_section()
                    => Load_and_save(Ini, Configuration, ExpectedIni);
            }
        }

        [TestClass]
        public sealed class With_configured_invalid_line_behavior
        {
            private const string Ini = """
                                       [Section]
                                       <Invalid line>
                                       Property=Value
                                       """;

            [TestClass]
            public sealed class When_fail_behavior
            {
                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Strict
                                                                                                 .WithInvalidLineBehavior(InvalidLineBehavior.Fail);

                [TestMethod]
                public void Throws_persistence_exception_on_invalid_line()
                {
                    using var textReader = new StringReader(Ini);

                    Assert.ThrowsException<PersistenceException>(() => IniDocument.LoadFrom(textReader, Configuration));
                }
            }

            [TestClass]
            public sealed class When_ignore_behavior
            {
                private const string ExpectedIni = """
                                                   [Section]
                                                   Property=Value

                                                   """;

                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Strict
                                                                                                 .WithInvalidLineBehavior(InvalidLineBehavior.Ignore);

                [TestMethod]
                public void Ignores_invalid_line()
                    => Load_and_save(Ini, Configuration, ExpectedIni);
            }
        }

        private static void Load_and_save(string sourceIni, IniDocumentConfiguration configuration, string expectedIni)
        {
            using var reader = new StringReader(sourceIni);

            var target = IniDocument.LoadFrom(reader, configuration);

            var stringBuilder = new StringBuilder();
            using var writer = new StringWriter(stringBuilder);

            target.SaveTo(writer);

            Assert.AreEqual(expectedIni, stringBuilder.ToString());
        }
    }

    [TestClass]
    public sealed class SaveTo
    {
        private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Strict;

        [TestMethod]
        public void Throws_argument_null_exception_when_text_writer_is_null()
        {
            var target = Make.EmptyTarget();

            Assert.ThrowsException<ArgumentNullException>(() => target.SaveTo(null));
        }

        [TestMethod]
        public void Throws_persistence_exception_containing_object_disposed_exception_when_text_writer_is_already_disposed()
        {
            var expected = PersistenceExceptionAssertion.Create<ObjectDisposedException>();
            using var textWriter = new StringWriter();
            textWriter.Dispose();

            var target = Make.EmptyTarget();

            var actual = Assert.ThrowsException<PersistenceException>(() => target.SaveTo(textWriter));

            Assert.AreEqual(expected, actual);
        }

        [DataTestMethod]
        [DynamicData(nameof(Saves_the_ini_document_to_text_writer_test_cases), DynamicDataSourceType.Method)]
        public void Saves_the_ini_document_to_text_writer(IniDocumentConfiguration configuration)
        {
            var expected = Make.SampleString(configuration);
            var stringBuilder = new StringBuilder();
            using var textWriter = new StringWriter(stringBuilder);

            var target = Make.SampleTarget(configuration);

            target.SaveTo(textWriter);
            textWriter.Flush();

            var actual = stringBuilder.ToString();

            Assert.AreEqual(expected, actual);
        }

        public static IEnumerable<object[]> Saves_the_ini_document_to_text_writer_test_cases()
        {
            yield return [Configuration];
            yield return [Configuration.WithSectionBeginningDelimiter('<')];
            yield return [Configuration.WithSectionEndDelimiter('>')];
            yield return [Configuration.WithPropertyAssignmentSeparator(':')];
            yield return [Configuration.WithPropertyAssignmentSpacer(PropertyAssignmentSpacer.Space)];
        }
    }

    [TestClass]
    public sealed class EnumerateSections
    {
        private static SectionName[] SectionNames => ["Section1", "AnotherSection2", "SomeSectionA"];

        [TestMethod]
        public void Enumerates_the_names_of_all_contained_sections()
        {
            var expected = SectionNames;

            var target = Make.EmptySectionsTarget(expected);

            var actual = target.EnumerateSections().ToImmutableArray();

            CollectionAssert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public sealed class EnumerateProperties
    {
        private static PropertyKey[] PropertyKeys => ["Property1", "AnotherProperty2", "SomePropertyA"];

        [TestClass]
        public sealed class With_sectionName
        {
            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.EnumerateProperties(null));
            }

            [TestMethod]
            public void Enumerates_the_keys_of_all_properties_contained_in_the_specified_section()
            {
                var expected = PropertyKeys;
                var target = Make.SingleDefaultSectionTarget(expected);

                var actual = target.EnumerateProperties(DefaultSectionName).ToImmutableArray();

                CollectionAssert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Default.WithPropertyEnumerationMode(PropertyEnumerationMode.Fail);

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var sectionName = NonexistentSectionName;
                    var expected = new SectionNotFoundExceptionAssertion(sectionName);

                    var target = Make.EmptyTarget(Configuration);

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.EnumerateProperties(sectionName));

                    Assert.AreEqual(expected, actual);
                }
            }

            [TestClass]
            public sealed class When_fallback_mode
            {
                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Default.WithPropertyEnumerationMode(PropertyEnumerationMode.Fallback);

                [TestMethod]
                public void Enumerates_an_empty_collection_when_section_does_not_exist()
                {
                    var expected = Enumerable.Empty<PropertyKey>().ToImmutableArray();

                    var target = Make.EmptyTarget(Configuration);

                    var actual = target.EnumerateProperties(NonexistentSectionName).ToImmutableArray();

                    CollectionAssert.AreEqual(expected, actual);
                }
            }
        }

        [TestClass]
        public sealed class With_sectionName_and_mode
        {
            private static PropertyEnumerationMode DummyMode => PropertyEnumerationMode.Fail;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.EnumerateProperties(null, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_mode_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.EnumerateProperties(DummySectionName, null));
            }

            [DataTestMethod]
            [DynamicData(nameof(Modes), DynamicDataSourceType.Method)]
            public void Enumerates_the_keys_of_all_properties_contained_in_the_specified_section(PropertyEnumerationMode mode)
            {
                var expected = PropertyKeys;
                var target = Make.SingleDefaultSectionTarget(expected);

                var actual = target.EnumerateProperties(DefaultSectionName, mode).ToImmutableArray();

                CollectionAssert.AreEqual(expected, actual);
            }

            public static IEnumerable<object[]> Modes()
            {
                yield return [PropertyEnumerationMode.Fail];
                yield return [PropertyEnumerationMode.Fallback];
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static PropertyEnumerationMode Mode => PropertyEnumerationMode.Fail;

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var target = Make.EmptyTarget();

                    Assert.ThrowsException<SectionNotFoundException>(() => target.EnumerateProperties(NonexistentSectionName, Mode));
                }
            }

            [TestClass]
            public sealed class When_fallback_mode
            {
                private static PropertyEnumerationMode Mode => PropertyEnumerationMode.Fallback;

                [TestMethod]
                public void Enumerates_an_empty_collection_when_section_does_not_exist()
                {
                    var expected = Enumerable.Empty<PropertyKey>().ToImmutableArray();

                    var target = Make.EmptyTarget();

                    var actual = target.EnumerateProperties(NonexistentSectionName, Mode).ToImmutableArray();

                    CollectionAssert.AreEqual(expected, actual);
                }
            }
        }
    }

    [TestClass]
    public sealed class ReadProperty
    {
        [TestClass]
        public sealed class With_sectionName_and_propertyKey
        {
            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadProperty(null, DummyPropertyKey));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadProperty(DummySectionName, null));
            }

            [DataTestMethod]
            [DataRow("Property value", DisplayName = "Actual value")]
            [DataRow("",               DisplayName = "Empty string when property does exist but has no value")]
            public void Reads_the_value_of_the_property_contained_in_the_specified_section(string propertyValue)
            {
                var expected = propertyValue;

                var target = Make.SingleDefaultPropertyTarget(expected);

                var actual = target.ReadProperty(DefaultSectionName, DefaultPropertyKey);

                Assert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Default.WithPropertyReadMode(PropertyReadMode.Fail);

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var sectionName = NonexistentSectionName;
                    var expected = new SectionNotFoundExceptionAssertion(sectionName);

                    var target = Make.EmptyTarget(Configuration);

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.ReadProperty(sectionName, DefaultPropertyKey));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Throws_property_not_found_exception_when_section_exists_but_property_does_not_exist()
                {
                    var propertyKey = NonexistentPropertyKey;
                    var expected = new PropertyNotFoundExceptionAssertion(propertyKey);

                    var target = Make.SingleDefaultPropertyTarget(Configuration, DefaultPropertyValue);

                    var actual = Assert.ThrowsException<PropertyNotFoundException>(() => target.ReadProperty(DefaultSectionName, propertyKey));

                    Assert.AreEqual(expected, actual);
                }
            }

            [TestClass]
            public sealed class When_fallback_mode
            {
                private static PropertyValue FallbackPropertyValue => "Fallback";

                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Default.WithPropertyReadMode(PropertyReadMode.CustomFallback(FallbackPropertyValue));

                [TestMethod]
                public void Returns_the_fallback_property_value_when_section_does_not_exist()
                {
                    var expected = FallbackPropertyValue;

                    var target = Make.EmptyTarget(Configuration);

                    var actual = target.ReadProperty(NonexistentSectionName, DefaultPropertyKey);

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Returns_the_fallback_property_value_when_section_exists_but_property_does_not_exist()
                {
                    var expected = FallbackPropertyValue;

                    var target = Make.SingleDefaultPropertyTarget(Configuration, DefaultPropertyValue);

                    var actual = target.ReadProperty(DefaultSectionName, NonexistentPropertyKey);

                    Assert.AreEqual(expected, actual);
                }
            }
        }

        [TestClass]
        public sealed class With_sectionName_and_propertyKey_and_mode
        {
            private static PropertyReadMode DummyMode => PropertyReadMode.Fail;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadProperty(null, DummyPropertyKey, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadProperty(DummySectionName, null, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_mode_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadProperty(DummySectionName, DummyPropertyKey, null));
            }

            [DataTestMethod]
            [DynamicData(nameof(Modes), DynamicDataSourceType.Method)]
            public void Reads_the_value_of_the_property_contained_in_the_specified_section(PropertyValue expected, PropertyReadMode mode)
            {
                var target = Make.SingleDefaultPropertyTarget(expected);

                var actual = target.ReadProperty(DefaultSectionName, DefaultPropertyKey, mode);

                Assert.AreEqual(expected, actual);
            }

            public static IEnumerable<object[]> Modes()
            {
                yield return [EmptyPropertyValue, PropertyReadMode.Fallback];
                yield return [EmptyPropertyValue, PropertyReadMode.Fail];

                yield return [DefaultPropertyValue, PropertyReadMode.Fallback];
                yield return [DefaultPropertyValue, PropertyReadMode.Fail];
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static PropertyReadMode Mode => PropertyReadMode.Fail;

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var target = Make.EmptyTarget();

                    Assert.ThrowsException<SectionNotFoundException>(() => target.ReadProperty(NonexistentSectionName, DefaultPropertyKey, Mode));
                }

                [TestMethod]
                public void Throws_property_not_found_exception_when_section_exists_but_property_does_not_exist()
                {
                    var target = Make.SingleDefaultPropertyTarget(DefaultPropertyValue);

                    Assert.ThrowsException<PropertyNotFoundException>(() => target.ReadProperty(DefaultSectionName, NonexistentPropertyKey, Mode));
                }
            }

            [TestClass]
            public sealed class When_fallback_mode
            {
                private static PropertyValue FallbackPropertyValue => "Fallback";

                private static PropertyReadMode Mode => PropertyReadMode.CustomFallback(FallbackPropertyValue);

                [TestMethod]
                public void Returns_the_fallback_property_value_when_section_does_not_exist()
                {
                    var expected = FallbackPropertyValue;

                    var target = Make.EmptyTarget();

                    var actual = target.ReadProperty(NonexistentSectionName, DefaultPropertyKey, Mode);

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Returns_the_fallback_property_value_when_section_exists_but_property_does_not_exist()
                {
                    var expected = FallbackPropertyValue;

                    var target = Make.SingleDefaultPropertyTarget(DefaultPropertyValue);

                    var actual = target.ReadProperty(DefaultSectionName, NonexistentPropertyKey, Mode);

                    Assert.AreEqual(expected, actual);
                }
            }
        }
    }

    [TestClass]
    public sealed class WriteProperty
    {
        private static PropertyReadMode ReadMode => PropertyReadMode.Fail;

        [TestClass]
        public sealed class With_sectionName_and_propertyKey_and_propertyValue
        {
            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.WriteProperty(null, DummyPropertyKey, DummyPropertyValue));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.WriteProperty(DummySectionName, null, DummyPropertyValue));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_value_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.WriteProperty(DummySectionName, DummyPropertyKey, null));
            }

            private static void Writes_property(IniDocument target)
            {
                var expected = DefaultPropertyValue;

                target.WriteProperty(DefaultSectionName, DefaultPropertyKey, expected);

                var actual = target.ReadProperty(DefaultSectionName, DefaultPropertyKey, ReadMode);

                Assert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_create_mode
            {
                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Default.WithPropertyWriteMode(PropertyWriteMode.Create);

                [TestMethod]
                public void Creates_property_with_given_property_value_when_section_does_not_exist()
                    => Writes_property(Make.EmptyTarget(Configuration));

                [TestMethod]
                public void Creates_property_with_given_property_value_when_section_exists_but_property_does_not_exist()
                    => Writes_property(Make.EmptySectionsTarget(Configuration, DefaultSectionName));

                [TestMethod]
                public void Overwrites_existing_property_with_given_property_value()
                    => Writes_property(Make.SingleDefaultPropertyTarget(Configuration, EmptyPropertyValue));
            }

            [TestClass]
            public sealed class When_update_mode
            {
                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Default.WithPropertyWriteMode(PropertyWriteMode.Update);

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var expected = new SectionNotFoundExceptionAssertion(NonexistentSectionName);
                    var target = Make.EmptyTarget(Configuration);

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.WriteProperty(NonexistentSectionName, DefaultPropertyKey, DefaultPropertyValue));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Throws_property_not_found_exception_when_property_does_not_exist()
                {
                    var expected = new PropertyNotFoundExceptionAssertion(NonexistentPropertyKey);
                    var target = Make.SingleDefaultPropertyTarget(Configuration, DefaultPropertyValue);

                    var actual = Assert.ThrowsException<PropertyNotFoundException>(() => target.WriteProperty(DefaultSectionName, NonexistentPropertyKey, DefaultPropertyValue));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Updates_existing_property_to_given_property_value()
                    => Writes_property(Make.SingleDefaultPropertyTarget(Configuration, EmptyPropertyValue));
            }
        }

        [TestClass]
        public sealed class With_sectionName_and_propertyKey_and_propertyValue_and_mode
        {
            private static PropertyWriteMode DummyMode => PropertyWriteMode.Update;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.WriteProperty(null, DummyPropertyKey, DummyPropertyValue, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.WriteProperty(DummySectionName, null, DummyPropertyValue, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_value_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.WriteProperty(DummySectionName, DummyPropertyKey, null, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_mode_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.WriteProperty(DummySectionName, DummyPropertyKey, DummyPropertyValue, null));
            }

            private static void Writes_property(IniDocument target, PropertyWriteMode mode)
            {
                var expected = DefaultPropertyValue;

                target.WriteProperty(DefaultSectionName, DefaultPropertyKey, expected, mode);

                var actual = target.ReadProperty(DefaultSectionName, DefaultPropertyKey, ReadMode);

                Assert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_create_mode
            {
                private static PropertyWriteMode Mode => PropertyWriteMode.Create;

                [TestMethod]
                public void Creates_property_with_given_property_value_when_section_does_not_exist()
                    => Writes_property(Make.EmptyTarget(), Mode);

                [TestMethod]
                public void Creates_property_with_given_property_value_when_section_exists_but_property_does_not_exist()
                    => Writes_property(Make.EmptySectionsTarget(DefaultSectionName), Mode);

                [TestMethod]
                public void Overwrites_existing_property_with_given_property_value()
                    => Writes_property(Make.SingleDefaultPropertyTarget(EmptyPropertyValue), Mode);
            }

            [TestClass]
            public sealed class When_update_mode
            {
                private static PropertyWriteMode Mode => PropertyWriteMode.Update;

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var expected = new SectionNotFoundExceptionAssertion(NonexistentSectionName);
                    var target = Make.EmptyTarget();

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.WriteProperty(NonexistentSectionName, DefaultPropertyKey, DefaultPropertyValue, Mode));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Throws_property_not_found_exception_when_property_does_not_exist()
                {
                    var expected = new PropertyNotFoundExceptionAssertion(NonexistentPropertyKey);
                    var target = Make.SingleDefaultPropertyTarget(DefaultPropertyValue);

                    var actual = Assert.ThrowsException<PropertyNotFoundException>(() => target.WriteProperty(DefaultSectionName, NonexistentPropertyKey, DefaultPropertyValue, Mode));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Updates_existing_property_to_given_property_value()
                    => Writes_property(Make.SingleDefaultPropertyTarget(EmptyPropertyValue), Mode);
            }
        }
    }

    [TestClass]
    public sealed class DeleteProperty
    {
        [TestClass]
        public sealed class With_sectionName_and_propertyKey
        {
            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteProperty(null, DummyPropertyKey));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteProperty(DummySectionName, null));
            }

            private static void DeletesProperty(IniDocumentConfiguration configuration)
            {
                var expected = new[] { DummyPropertyKey };
                var target = Make.SingleDefaultSectionTarget(configuration, DummyPropertyKey, DefaultPropertyKey);

                target.DeleteProperty(DefaultSectionName, DefaultPropertyKey);

                var actual = target.EnumerateProperties(DefaultSectionName).ToArray();

                CollectionAssert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Default.WithPropertyDeletionMode(PropertyDeletionMode.Fail);

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var expected = new SectionNotFoundExceptionAssertion(NonexistentSectionName);
                    var target = Make.EmptyTarget(Configuration);

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.DeleteProperty(NonexistentSectionName, DummyPropertyKey));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Throws_property_not_found_exception_when_property_does_not_exist()
                {
                    var expected = new PropertyNotFoundExceptionAssertion(NonexistentPropertyKey);
                    var target = Make.EmptySectionsTarget(Configuration, DefaultSectionName);

                    var actual = Assert.ThrowsException<PropertyNotFoundException>(() => target.DeleteProperty(DefaultSectionName, NonexistentPropertyKey));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Deletes_property()
                    => DeletesProperty(Configuration);
            }

            [TestClass]
            public sealed class When_ignore_mode
            {
                private static IniDocumentConfiguration Configuration => IniDocumentConfiguration.Default.WithPropertyDeletionMode(PropertyDeletionMode.Ignore);

                [TestMethod]
                public void Ignores_when_section_does_not_exist()
                {
                    var target = Make.EmptyTarget(Configuration);

                    target.DeleteProperty(NonexistentSectionName, DummyPropertyKey);
                }

                [TestMethod]
                public void Ignores_when_property_does_not_exist()
                {
                    var target = Make.EmptySectionsTarget(Configuration, DefaultSectionName);

                    target.DeleteProperty(DefaultSectionName, NonexistentPropertyKey);
                }

                [TestMethod]
                public void Deletes_property()
                    => DeletesProperty(Configuration);
            }
        }

        [TestClass]
        public sealed class With_sectionName_and_propertyKey_and_mode
        {
            private static PropertyDeletionMode DummyMode => PropertyDeletionMode.Fail;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteProperty(null, DummyPropertyKey, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteProperty(DummySectionName, null, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_mode_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteProperty(DummySectionName, DummyPropertyKey, null));
            }

            private static void DeletesProperty(PropertyDeletionMode mode)
            {
                var expected = new[] { DummyPropertyKey };
                var target = Make.SingleDefaultSectionTarget(DummyPropertyKey, DefaultPropertyKey);

                target.DeleteProperty(DefaultSectionName, DefaultPropertyKey, mode);

                var actual = target.EnumerateProperties(DefaultSectionName).ToArray();

                CollectionAssert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static PropertyDeletionMode Mode => PropertyDeletionMode.Fail;

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var expected = new SectionNotFoundExceptionAssertion(NonexistentSectionName);
                    var target = Make.EmptyTarget();

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.DeleteProperty(NonexistentSectionName, DummyPropertyKey, Mode));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Throws_property_not_found_exception_when_property_does_not_exist()
                {
                    var expected = new PropertyNotFoundExceptionAssertion(NonexistentPropertyKey);
                    var target = Make.EmptySectionsTarget(DefaultSectionName);

                    var actual = Assert.ThrowsException<PropertyNotFoundException>(() => target.DeleteProperty(DefaultSectionName, NonexistentPropertyKey, Mode));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Deletes_property()
                    => DeletesProperty(Mode);
            }

            [TestClass]
            public sealed class When_ignore_mode
            {
                private static PropertyDeletionMode Mode => PropertyDeletionMode.Ignore;

                [TestMethod]
                public void Ignores_when_section_does_not_exist()
                {
                    var target = Make.EmptyTarget();

                    target.DeleteProperty(NonexistentSectionName, DummyPropertyKey, Mode);
                }

                [TestMethod]
                public void Ignores_when_property_does_not_exist()
                {
                    var target = Make.EmptySectionsTarget(DefaultSectionName);

                    target.DeleteProperty(DefaultSectionName, NonexistentPropertyKey, Mode);
                }

                [TestMethod]
                public void Deletes_property()
                    => DeletesProperty(Mode);
            }
        }
    }

    [TestClass]
    public sealed class DeleteSection
    {
        [TestClass]
        public sealed class With_sectionName
        {
            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteSection(null));
            }

            private static void DeletesSection(IniDocumentConfiguration configuration)
            {
                var expected = new[] { DummySectionName };
                var target = Make.EmptySectionsTarget(configuration, DummySectionName, DefaultSectionName);

                target.DeleteSection(DefaultSectionName);

                var actual = target.EnumerateSections().ToArray();

                CollectionAssert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithSectionDeletionMode(SectionDeletionMode.Fail);

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var expected = new SectionNotFoundExceptionAssertion(NonexistentSectionName);
                    var target = Make.EmptyTarget(Configuration);

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.DeleteSection(NonexistentSectionName));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Deletes_section()
                    => DeletesSection(Configuration);
            }

            [TestClass]
            public sealed class When_ignore_mode
            {
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithSectionDeletionMode(SectionDeletionMode.Ignore);

                [TestMethod]
                public void Ignores_when_section_does_not_exist()
                {
                    var target = Make.EmptyTarget(Configuration);

                    target.DeleteSection(NonexistentSectionName);
                }

                [TestMethod]
                public void Deletes_section()
                    => DeletesSection(Configuration);
            }
        }

        [TestClass]
        public sealed class With_sectionName_and_mode
        {
            private static SectionDeletionMode DummyMode => SectionDeletionMode.Fail;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteSection(null, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_mode_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteSection(DummySectionName, null));
            }

            private static void DeletesSection(SectionDeletionMode mode)
            {
                var expected = new[] { DummySectionName };
                var target = Make.EmptySectionsTarget(DummySectionName, DefaultSectionName);

                target.DeleteSection(DefaultSectionName, mode);

                var actual = target.EnumerateSections().ToArray();

                CollectionAssert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static SectionDeletionMode Mode => SectionDeletionMode.Fail;

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var expected = new SectionNotFoundExceptionAssertion(NonexistentSectionName);
                    var target = Make.EmptyTarget();

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.DeleteSection(NonexistentSectionName, Mode));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Deletes_section()
                    => DeletesSection(Mode);
            }

            [TestClass]
            public sealed class When_ignore_mode
            {
                private static SectionDeletionMode Mode => SectionDeletionMode.Ignore;

                [TestMethod]
                public void Ignores_when_section_does_not_exist()
                {
                    var target = Make.EmptyTarget();

                    target.DeleteSection(NonexistentSectionName, Mode);
                }

                [TestMethod]
                public void Deletes_section()
                    => DeletesSection(Mode);
            }
        }
    }

    private static class Make
    {
        public static IniDocument EmptyTarget()
            => EmptyTarget(DefaultConfiguration);

        public static IniDocument EmptyTarget(IniDocumentConfiguration configuration)
            => IniDocument.CreateEmpty(configuration);

        public static IniDocument SingleDefaultSectionTarget(params PropertyKey[] propertyKeys)
            => SingleDefaultSectionTarget(DefaultConfiguration, propertyKeys);

        public static IniDocument SingleDefaultSectionTarget(IniDocumentConfiguration configuration, params PropertyKey[] propertyKeys)
            => Target(configuration, Section.Create(DefaultSectionName, propertyKeys.Select(Property.Create)));

        public static IniDocument SingleDefaultPropertyTarget(PropertyValue propertyValue)
            => SingleDefaultPropertyTarget(DefaultConfiguration, propertyValue);

        public static IniDocument SingleDefaultPropertyTarget(IniDocumentConfiguration configuration, PropertyValue propertyValue)
            => Target(configuration, Section.Create(DefaultSectionName, Property.Create(DefaultPropertyKey, propertyValue)));

        public static IniDocument EmptySectionsTarget(params SectionName[] sectionNames)
            => EmptySectionsTarget(DefaultConfiguration, sectionNames);

        public static IniDocument EmptySectionsTarget(IniDocumentConfiguration configuration, params SectionName[] sectionNames)
            => Target(configuration, sectionNames.Select(Section.CreateEmpty));

        public static IniDocument SampleTarget(IniDocumentConfiguration configuration)
            => Target(configuration,
                      Section.Create("Section1", Property.Create("PropertyA", "Value A")),
                      Section.Create("Section2", Property.Create("PropertyB", "Value B")));

        public static string SampleString(IniDocumentConfiguration configuration)
            => new IniDocumentBuilder(configuration).AppendSectionLine("Section1")
                                                    .AppendPropertyLine("PropertyA", "Value A")
                                                    .AppendEmptyLine()
                                                    .AppendSectionLine("Section2")
                                                    .AppendPropertyLine("PropertyB", "Value B")
                                                    .ToString();

        private static IniDocument Target(IniDocumentConfiguration configuration, params Section[] sections)
            => Target(configuration, sections.AsEnumerable());

        private static IniDocument Target(IniDocumentConfiguration configuration, IEnumerable<Section> sections)
            => sections.Aggregate(new IniDocumentBuilder(configuration), (builder, section) => section.ApplyTo(builder)).Build();

        private sealed class IniDocumentBuilder
        {
            private readonly StringBuilder stringBuilder = new();

            private readonly IniDocumentConfiguration configuration;

            public IniDocumentBuilder(IniDocumentConfiguration configuration)
            {
                this.configuration = configuration;
            }

            public IniDocumentBuilder AppendEmptyLine()
            {
                this.stringBuilder.AppendLine();

                return this;
            }

            public IniDocumentBuilder AppendSectionLine(SectionName sectionName)
            {
                this.stringBuilder.AppendLine($"{this.configuration.SectionBeginningDelimiter}{sectionName}{this.configuration.SectionEndDelimiter}");

                return this;
            }

            public IniDocumentBuilder AppendPropertyLine(PropertyKey propertyKey, PropertyValue propertyValue)
            {
                this.stringBuilder.AppendLine($"{propertyKey}{this.configuration.PropertyAssignmentSpacer}{this.configuration.PropertyAssignmentSeparator}{this.configuration.PropertyAssignmentSpacer}{propertyValue}");

                return this;
            }

            public override string ToString()
                => this.stringBuilder.ToString();

            public IniDocument Build()
            {
                using var stringReader = new StringReader(this.ToString());
                return IniDocument.LoadFrom(stringReader, this.configuration);
            }
        }

        private sealed record Section(SectionName SectionName, ImmutableArray<Property> Properties)
        {
            public static Section CreateEmpty(SectionName sectionName)
                => Create(sectionName);

            public static Section Create(SectionName sectionName, params Property[] properties)
                => Create(sectionName, properties.AsEnumerable());

            public static Section Create(SectionName sectionName, IEnumerable<Property> properties)
                => new(sectionName, [..properties]);

            public IniDocumentBuilder ApplyTo(IniDocumentBuilder builder)
                => this.Properties.Aggregate(builder.AppendSectionLine(this.SectionName), (currentBuilder, property) => property.ApplyTo(currentBuilder));
        }

        private sealed record Property(PropertyKey PropertyKey, PropertyValue PropertyValue)
        {
            public static Property Create(PropertyKey propertyKey)
                => Create(propertyKey, "Default");

            public static Property Create(PropertyKey propertyKey, PropertyValue propertyValue)
                => new(propertyKey, propertyValue);

            public IniDocumentBuilder ApplyTo(IniDocumentBuilder builder)
                => builder.AppendPropertyLine(this.PropertyKey, this.PropertyValue);
        }
    }
}
