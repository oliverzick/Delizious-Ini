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
    private static IniDocumentConfiguration DefaultConfiguration => IniDocumentConfiguration.Strict;

    private static SectionName DummySectionName => "Dummy";
    private static PropertyKey DummyPropertyKey => "Dummy";
    private static PropertyValue DummyPropertyValue => "Dummy";

    private static SectionName NonexistentSectionName => "NonexistentSection";
    private static PropertyKey NonexistentPropertyKey => "NonexistentProperty";

    private static SectionName DefaultSectionName => "Section";
    private static PropertyKey DefaultPropertyKey => "Property";
    private static PropertyValue DefaultPropertyValue => "Value";

    private static PropertyValue EmptyPropertyValue => PropertyValue.None;

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

            [TestClass]
            public sealed class When_fail_behavior
            {
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Fail);

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

                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Ignore);

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

                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Override);

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

                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithDuplicateSectionBehavior(DuplicateSectionBehavior.Merge)
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

                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithDuplicateSectionBehavior(DuplicateSectionBehavior.Merge)
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
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithDuplicateSectionBehavior(DuplicateSectionBehavior.Fail);

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

                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithDuplicateSectionBehavior(DuplicateSectionBehavior.Merge);

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
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithInvalidLineBehavior(InvalidLineBehavior.Fail);

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

                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithInvalidLineBehavior(InvalidLineBehavior.Ignore);

                [TestMethod]
                public void Ignores_invalid_line()
                    => Load_and_save(Ini, Configuration, ExpectedIni);
            }
        }

        [TestClass]
        public sealed class With_configured_invalid_line_behavior_and_section_name_regex
        {
            private const string Ini = """
                                       [ValidSection]
                                       Property=Value 1

                                       [InvalidSection!]
                                       UnluckyProperty=Value 2

                                       [AnotherValidSection]
                                       AnotherProperty=Value 3
                                       """;

            private static SectionNameRegex SectionNameRegex => SectionNameRegex.Create(@"\w+");

            [TestClass]
            public sealed class When_fail_behavior
            {
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithInvalidLineBehavior(InvalidLineBehavior.Fail)
                                                                                             .WithSectionNameRegex(SectionNameRegex);

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
                                                   [ValidSection]
                                                   Property=Value 1
                                                   UnluckyProperty=Value 2

                                                   [AnotherValidSection]
                                                   AnotherProperty=Value 3

                                                   """;

                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithInvalidLineBehavior(InvalidLineBehavior.Ignore)
                                                                                             .WithSectionNameRegex(SectionNameRegex);

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
        private static IniDocumentConfiguration Configuration => DefaultConfiguration;

        [TestMethod]
        public void Throws_argument_null_exception_when_text_writer_is_null()
        {
            var target = Make.EmptyTarget(Configuration);

            Assert.ThrowsException<ArgumentNullException>(() => target.SaveTo(null));
        }

        [TestMethod]
        public void Throws_persistence_exception_containing_object_disposed_exception_when_text_writer_is_already_disposed()
        {
            var expected = PersistenceExceptionAssertion.Create<ObjectDisposedException>();
            using var textWriter = new StringWriter();
            textWriter.Dispose();

            var target = Make.EmptyTarget(Configuration);

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
            yield return [Configuration.WithNewlineString(NewlineString.Environment)];
            yield return [Configuration.WithNewlineString(NewlineString.Unix)];
            yield return [Configuration.WithNewlineString(NewlineString.Windows)];
            yield return [Configuration.WithSectionBeginningDelimiter('<')];
            yield return [Configuration.WithSectionEndDelimiter('>')];
            yield return [Configuration.WithPropertyAssignmentSeparator(':')];
            yield return [Configuration.WithPropertyAssignmentSpacer(PropertyAssignmentSpacer.Space)];
        }
    }

    [TestClass]
    public sealed class Clone
    {
        private const string Ini = """
                                   [Section1]
                                   PropertyA=Value A

                                   [Section2]
                                   PropertyB=Value B
                                   """;

        private static IniDocumentConfiguration Configuration => DefaultConfiguration;

        [TestMethod]
        public void Cloned_instance_is_not_the_same_as_the_original_instance()
        {
            var original = Make.Target(Ini, Configuration);

            var clone = original.Clone();

            Assert.AreNotSame(original, clone);
        }

        [TestMethod]
        public void Cloned_instance_represents_same_content_as_the_original_instance()
        {
            var original = Make.Target(Ini, Configuration);

            var clone = original.Clone();

            using var originalWriter = new StringWriter();
            original.SaveTo(originalWriter);

            using var cloneWriter = new StringWriter();
            clone.SaveTo(cloneWriter);

            Assert.AreEqual(originalWriter.ToString(), cloneWriter.ToString());
        }

        [TestMethod]
        public void Changes_in_cloned_instance_do_not_affect_the_original_instance()
        {
            var original = Make.Target(Ini, Configuration);

            var clone = original.Clone();
            clone.WriteProperty("CloneSection", "CloneProperty", "Clone value", PropertyWriteMode.Create);

            using var originalWriter = new StringWriter();
            original.SaveTo(originalWriter);

            using var cloneWriter = new StringWriter();
            clone.SaveTo(cloneWriter);

            Assert.AreNotEqual(originalWriter.ToString(), cloneWriter.ToString());
        }
    }

    [TestClass]
    public sealed class EnumerateSections
    {
        private const string Ini = """
                                   [Section1]
                                   [AnotherSection2]
                                   [SomeSectionA]
                                   """;

        private static IniDocumentConfiguration Configuration => DefaultConfiguration;

        private static ImmutableArray<SectionName> SectionNames => ["Section1", "AnotherSection2", "SomeSectionA"];

        [TestMethod]
        public void Enumerates_the_names_of_all_contained_sections()
        {
            var expected = SectionNames;

            var target = Make.Target(Ini, Configuration);

            var actual = target.EnumerateSections().ToImmutableArray();

            CollectionAssert.AreEqual(expected, actual);
        }
    }

    [TestClass]
    public sealed class EnumerateProperties
    {
        private const string Ini = """
                                   [Section]
                                   Property1=
                                   AnotherProperty2=
                                   SomePropertyA=

                                   [AnotherSection]
                                   Property2=
                                   """;

        private static IniDocumentConfiguration Configuration => DefaultConfiguration;

        private static SectionName SectionName => "Section";

        private static PropertyKey[] PropertyKeys => ["Property1", "AnotherProperty2", "SomePropertyA"];

        [TestClass]
        public sealed class With_sectionName
        {
            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget(DefaultConfiguration);

                Assert.ThrowsException<ArgumentNullException>(() => target.EnumerateProperties(null));
            }

            [TestMethod]
            public void Enumerates_the_keys_of_all_properties_contained_in_the_specified_section()
            {
                var expected = PropertyKeys;
                var target = Make.Target(Ini, Configuration);

                var actual = target.EnumerateProperties(SectionName).ToImmutableArray();

                CollectionAssert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithPropertyEnumerationMode(PropertyEnumerationMode.Fail);

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
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithPropertyEnumerationMode(PropertyEnumerationMode.Fallback);

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
            private static IniDocumentConfiguration Configuration => DefaultConfiguration;

            private static PropertyEnumerationMode DummyMode => PropertyEnumerationMode.Fail;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.EnumerateProperties(null, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_mode_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.EnumerateProperties(DummySectionName, null));
            }

            [DataTestMethod]
            [DynamicData(nameof(Modes), DynamicDataSourceType.Method)]
            public void Enumerates_the_keys_of_all_properties_contained_in_the_specified_section(PropertyEnumerationMode mode)
            {
                var expected = PropertyKeys;
                var target = Make.Target(Ini, Configuration);

                var actual = target.EnumerateProperties(SectionName, mode).ToImmutableArray();

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
                    var target = Make.EmptyTarget(Configuration);

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

                    var target = Make.EmptyTarget(Configuration);

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
                var target = Make.EmptyTarget(DefaultConfiguration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadProperty(null, DummyPropertyKey));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget(DefaultConfiguration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadProperty(DummySectionName, null));
            }

            [DataTestMethod]
            [DataRow("Property value", DisplayName = "Actual value")]
            [DataRow("",               DisplayName = "Empty string when property does exist but has no value")]
            public void Reads_the_value_of_the_property_contained_in_the_specified_section(string propertyValue)
            {
                var expected = propertyValue;

                var target = Make.SingleDefaultPropertyTarget(DefaultConfiguration, expected);

                var actual = target.ReadProperty(DefaultSectionName, DefaultPropertyKey);

                Assert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithPropertyReadMode(PropertyReadMode.Fail);

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

                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithPropertyReadMode(PropertyReadMode.CustomFallback(FallbackPropertyValue));

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
            private static IniDocumentConfiguration Configuration => DefaultConfiguration;

            private static PropertyReadMode DummyMode => PropertyReadMode.Fail;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadProperty(null, DummyPropertyKey, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadProperty(DummySectionName, null, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_mode_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadProperty(DummySectionName, DummyPropertyKey, null));
            }

            [DataTestMethod]
            [DynamicData(nameof(Modes), DynamicDataSourceType.Method)]
            public void Reads_the_value_of_the_property_contained_in_the_specified_section(PropertyValue expected, PropertyReadMode mode)
            {
                var target = Make.SingleDefaultPropertyTarget(Configuration, expected);

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
                    var target = Make.EmptyTarget(Configuration);

                    Assert.ThrowsException<SectionNotFoundException>(() => target.ReadProperty(NonexistentSectionName, DefaultPropertyKey, Mode));
                }

                [TestMethod]
                public void Throws_property_not_found_exception_when_section_exists_but_property_does_not_exist()
                {
                    var target = Make.SingleDefaultPropertyTarget(Configuration, DefaultPropertyValue);

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

                    var target = Make.EmptyTarget(Configuration);

                    var actual = target.ReadProperty(NonexistentSectionName, DefaultPropertyKey, Mode);

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Returns_the_fallback_property_value_when_section_exists_but_property_does_not_exist()
                {
                    var expected = FallbackPropertyValue;

                    var target = Make.SingleDefaultPropertyTarget(Configuration, DefaultPropertyValue);

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
                var target = Make.EmptyTarget(DefaultConfiguration);

                Assert.ThrowsException<ArgumentNullException>(() => target.WriteProperty(null, DummyPropertyKey, DummyPropertyValue));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget(DefaultConfiguration);

                Assert.ThrowsException<ArgumentNullException>(() => target.WriteProperty(DummySectionName, null, DummyPropertyValue));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_value_is_null()
            {
                var target = Make.EmptyTarget(DefaultConfiguration);

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
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithPropertyWriteMode(PropertyWriteMode.Create);

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
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithPropertyWriteMode(PropertyWriteMode.Update);

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
            private static IniDocumentConfiguration Configuration => DefaultConfiguration;

            private static PropertyWriteMode DummyMode => PropertyWriteMode.Update;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.WriteProperty(null, DummyPropertyKey, DummyPropertyValue, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.WriteProperty(DummySectionName, null, DummyPropertyValue, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_value_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.WriteProperty(DummySectionName, DummyPropertyKey, null, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_mode_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

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
                    => Writes_property(Make.EmptyTarget(Configuration), Mode);

                [TestMethod]
                public void Creates_property_with_given_property_value_when_section_exists_but_property_does_not_exist()
                    => Writes_property(Make.EmptySectionsTarget(Configuration, DefaultSectionName), Mode);

                [TestMethod]
                public void Overwrites_existing_property_with_given_property_value()
                    => Writes_property(Make.SingleDefaultPropertyTarget(Configuration, EmptyPropertyValue), Mode);
            }

            [TestClass]
            public sealed class When_update_mode
            {
                private static PropertyWriteMode Mode => PropertyWriteMode.Update;

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var expected = new SectionNotFoundExceptionAssertion(NonexistentSectionName);
                    var target = Make.EmptyTarget(Configuration);

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.WriteProperty(NonexistentSectionName, DefaultPropertyKey, DefaultPropertyValue, Mode));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Throws_property_not_found_exception_when_property_does_not_exist()
                {
                    var expected = new PropertyNotFoundExceptionAssertion(NonexistentPropertyKey);
                    var target = Make.SingleDefaultPropertyTarget(Configuration, DefaultPropertyValue);

                    var actual = Assert.ThrowsException<PropertyNotFoundException>(() => target.WriteProperty(DefaultSectionName, NonexistentPropertyKey, DefaultPropertyValue, Mode));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Updates_existing_property_to_given_property_value()
                    => Writes_property(Make.SingleDefaultPropertyTarget(Configuration, EmptyPropertyValue), Mode);
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
                var target = Make.EmptyTarget(DefaultConfiguration);

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteProperty(null, DummyPropertyKey));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget(DefaultConfiguration);

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
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithPropertyDeletionMode(PropertyDeletionMode.Fail);

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
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithPropertyDeletionMode(PropertyDeletionMode.Ignore);

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
            private static IniDocumentConfiguration Configuration => DefaultConfiguration;

            private static PropertyDeletionMode DummyMode => PropertyDeletionMode.Fail;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteProperty(null, DummyPropertyKey, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteProperty(DummySectionName, null, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_mode_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteProperty(DummySectionName, DummyPropertyKey, null));
            }

            private static void DeletesProperty(PropertyDeletionMode mode)
            {
                var expected = new[] { DummyPropertyKey };
                var target = Make.SingleDefaultSectionTarget(Configuration, DummyPropertyKey, DefaultPropertyKey);

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
                    var target = Make.EmptyTarget(Configuration);

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.DeleteProperty(NonexistentSectionName, DummyPropertyKey, Mode));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Throws_property_not_found_exception_when_property_does_not_exist()
                {
                    var expected = new PropertyNotFoundExceptionAssertion(NonexistentPropertyKey);
                    var target = Make.EmptySectionsTarget(Configuration, DefaultSectionName);

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
                    var target = Make.EmptyTarget(Configuration);

                    target.DeleteProperty(NonexistentSectionName, DummyPropertyKey, Mode);
                }

                [TestMethod]
                public void Ignores_when_property_does_not_exist()
                {
                    var target = Make.EmptySectionsTarget(Configuration, DefaultSectionName);

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
                var target = Make.EmptyTarget(DefaultConfiguration);

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
            private static IniDocumentConfiguration Configuration => DefaultConfiguration;

            private static SectionDeletionMode DummyMode => SectionDeletionMode.Fail;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteSection(null, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_mode_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.DeleteSection(DummySectionName, null));
            }

            private static void DeletesSection(SectionDeletionMode mode)
            {
                var expected = new[] { DummySectionName };
                var target = Make.EmptySectionsTarget(Configuration, DummySectionName, DefaultSectionName);

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
                    var target = Make.EmptyTarget(Configuration);

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
                    var target = Make.EmptyTarget(Configuration);

                    target.DeleteSection(NonexistentSectionName, Mode);
                }

                [TestMethod]
                public void Deletes_section()
                    => DeletesSection(Mode);
            }
        }
    }

    [TestClass]
    public sealed class ReadComment
    {
        [TestClass]
        public sealed class With_sectionName
        {
            private const string Ini = """
                                       ;This is a multiline
                                       ;
                                       ;comment.
                                       [Section]
                                       Property=Value

                                       [AnotherSection]
                                       Property=Another value
                                       """;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget(DefaultConfiguration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadComment(null));
            }

            private static void ReadsComment(IniDocumentConfiguration configuration)
            {
                Comment expected = """
                                   This is a multiline

                                   comment.
                                   """;

                using var reader = new StringReader(Ini);

                var target = IniDocument.LoadFrom(reader, configuration);

                var actual = target.ReadComment("Section");

                Assert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithCommentReadMode(CommentReadMode.Fail);

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var expected = new SectionNotFoundExceptionAssertion(NonexistentSectionName);
                    var target = Make.EmptyTarget(Configuration);

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.ReadComment(NonexistentSectionName));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Reads_comment()
                    => ReadsComment(Configuration);
            }

            [TestClass]
            public sealed class When_fallback_mode
            {
                private static Comment FallbackComment => "Fallback comment";

                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithCommentReadMode(CommentReadMode.CustomFallback(FallbackComment));

                [TestMethod]
                public void Provides_the_fallback_comment_when_section_does_not_exist()
                {
                    var expected = FallbackComment;

                    var target = Make.EmptyTarget(Configuration);

                    var actual = target.ReadComment(NonexistentSectionName);

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Reads_comment()
                    => ReadsComment(Configuration);
            }
        }

        [TestClass]
        public sealed class With_sectionName_and_mode
        {
            private const string Ini = """
                                       ;This is a multiline
                                       ;
                                       ;comment.
                                       [Section]
                                       Property=Value

                                       [AnotherSection]
                                       Property=Another value
                                       """;

            private static IniDocumentConfiguration Configuration => DefaultConfiguration;

            private static CommentReadMode DummyMode => CommentReadMode.Fail;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadComment(null, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_mode_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadComment(DummySectionName, null as CommentReadMode));
            }

            private static void ReadsComment(CommentReadMode mode)
            {
                Comment expected = """
                                   This is a multiline

                                   comment.
                                   """;

                using var reader = new StringReader(Ini);

                var target = IniDocument.LoadFrom(reader, Configuration);

                var actual = target.ReadComment("Section", mode);

                Assert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static CommentReadMode Mode => CommentReadMode.Fail;

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var expected = new SectionNotFoundExceptionAssertion(NonexistentSectionName);
                    var target = Make.EmptyTarget(Configuration);

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.ReadComment(NonexistentSectionName, Mode));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Reads_comment()
                    => ReadsComment(Mode);
            }

            [TestClass]
            public sealed class When_fallback_mode
            {
                private static Comment FallbackComment => "Fallback comment";

                private static CommentReadMode Mode => CommentReadMode.CustomFallback(FallbackComment);

                [TestMethod]
                public void Provides_the_fallback_comment_when_section_does_not_exist()
                {
                    var expected = FallbackComment;

                    var target = Make.EmptyTarget(Configuration);

                    var actual = target.ReadComment(NonexistentSectionName, Mode);

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Reads_comment()
                    => ReadsComment(Mode);
            }
        }

        [TestClass]
        public sealed class With_sectionName_and_propertyKey
        {
            private const string Ini = """
                                       [Section]
                                       ;This is a multiline
                                       ;
                                       ;comment.
                                       Property=Value

                                       [AnotherSection]
                                       Property=Another value
                                       """;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget(DefaultConfiguration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadComment(null, DummyPropertyKey));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget(DefaultConfiguration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadComment(DummySectionName, null as PropertyKey));
            }

            private static void ReadsComment(IniDocumentConfiguration configuration)
            {
                Comment expected = """
                                   This is a multiline

                                   comment.
                                   """;

                using var reader = new StringReader(Ini);

                var target = IniDocument.LoadFrom(reader, configuration);

                var actual = target.ReadComment("Section", "Property");

                Assert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithCommentReadMode(CommentReadMode.Fail);

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_not_exist()
                {
                    var expected = new SectionNotFoundExceptionAssertion(NonexistentSectionName);
                    var target = Make.EmptyTarget(Configuration);

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.ReadComment(NonexistentSectionName, DefaultPropertyKey));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Throws_property_not_found_exception_when_property_does_not_exist()
                {
                    var expected = new PropertyNotFoundExceptionAssertion(NonexistentPropertyKey);
                    var target = Make.SingleDefaultPropertyTarget(Configuration, DefaultPropertyValue);

                    var actual = Assert.ThrowsException<PropertyNotFoundException>(() => target.ReadComment(DefaultSectionName, NonexistentPropertyKey));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Reads_comment()
                    => ReadsComment(Configuration);
            }

            [TestClass]
            public sealed class When_fallback_mode
            {
                private static Comment FallbackComment => "Fallback comment";

                private static IniDocumentConfiguration Configuration => DefaultConfiguration.WithCommentReadMode(CommentReadMode.CustomFallback(FallbackComment));

                [TestMethod]
                public void Provides_the_fallback_comment_when_section_does_not_exist()
                {
                    var expected = FallbackComment;

                    var target = Make.EmptyTarget(Configuration);

                    var actual = target.ReadComment(NonexistentSectionName, DefaultPropertyKey);

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Provides_the_fallback_comment_when_property_does_not_exist()
                {
                    var expected = FallbackComment;

                    var target = Make.SingleDefaultPropertyTarget(Configuration, DefaultPropertyValue);

                    var actual = target.ReadComment(DefaultSectionName, NonexistentPropertyKey);

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Reads_comment()
                    => ReadsComment(Configuration);
            }
        }

        [TestClass]
        public sealed class With_sectionName_and_propertyKey_and_mode
        {
            private const string Ini = """
                                       [Section]
                                       ;This is a multiline
                                       ;
                                       ;comment.
                                       Property=Value

                                       [AnotherSection]
                                       Property=Another value
                                       """;

            private static IniDocumentConfiguration Configuration => DefaultConfiguration;

            private static CommentReadMode DummyMode => CommentReadMode.Fail;

            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadComment(null, DummyPropertyKey, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadComment(DummySectionName, null, DummyMode));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_mode_is_null()
            {
                var target = Make.EmptyTarget(Configuration);

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadComment(DummySectionName, DummyPropertyKey, null));
            }

            private static void ReadsComment(CommentReadMode mode)
            {
                Comment expected = """
                                   This is a multiline

                                   comment.
                                   """;

                using var reader = new StringReader(Ini);

                var target = IniDocument.LoadFrom(reader, Configuration);

                var actual = target.ReadComment("Section", "Property", mode);

                Assert.AreEqual(expected, actual);
            }

            [TestClass]
            public sealed class When_fail_mode
            {
                private static CommentReadMode Mode => CommentReadMode.Fail;

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_not_exist()
                {
                    var expected = new SectionNotFoundExceptionAssertion(NonexistentSectionName);
                    var target = Make.EmptyTarget(Configuration);

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.ReadComment(NonexistentSectionName, DefaultPropertyKey, Mode));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Throws_property_not_found_exception_when_property_does_not_exist()
                {
                    var expected = new PropertyNotFoundExceptionAssertion(NonexistentPropertyKey);
                    var target = Make.SingleDefaultPropertyTarget(Configuration, DefaultPropertyValue);

                    var actual = Assert.ThrowsException<PropertyNotFoundException>(() => target.ReadComment(DefaultSectionName, NonexistentPropertyKey, Mode));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Reads_comment()
                    => ReadsComment(Mode);
            }

            [TestClass]
            public sealed class When_fallback_mode
            {
                private static Comment FallbackComment => "Fallback comment";

                private static CommentReadMode Mode => CommentReadMode.CustomFallback(FallbackComment);

                [TestMethod]
                public void Provides_the_fallback_comment_when_section_does_not_exist()
                {
                    var expected = FallbackComment;

                    var target = Make.EmptyTarget(Configuration);

                    var actual = target.ReadComment(NonexistentSectionName, DefaultPropertyKey, Mode);

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Provides_the_fallback_comment_when_property_does_not_exist()
                {
                    var expected = FallbackComment;

                    var target = Make.SingleDefaultPropertyTarget(Configuration, DefaultPropertyValue);

                    var actual = target.ReadComment(DefaultSectionName, NonexistentPropertyKey, Mode);

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Reads_comment()
                    => ReadsComment(Mode);
            }
        }
    }

    private static class Make
    {
        public static IniDocument Target(string ini, IniDocumentConfiguration configuration)
        {
            using var reader = new StringReader(ini);

            return IniDocument.LoadFrom(reader, configuration);
        }

        public static IniDocument EmptyTarget(IniDocumentConfiguration configuration)
            => IniDocument.CreateEmpty(configuration);

        public static IniDocument SingleDefaultSectionTarget(IniDocumentConfiguration configuration, params PropertyKey[] propertyKeys)
            => Target(configuration, Section.Create(DefaultSectionName, propertyKeys.Select(Property.Create)));

        public static IniDocument SingleDefaultPropertyTarget(IniDocumentConfiguration configuration, PropertyValue propertyValue)
            => Target(configuration, Section.Create(DefaultSectionName, Property.Create(DefaultPropertyKey, propertyValue)));

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
                => this.AppendLine(string.Empty);

            public IniDocumentBuilder AppendSectionLine(SectionName sectionName)
                => this.AppendLine($"{this.configuration.SectionBeginningDelimiter}{sectionName}{this.configuration.SectionEndDelimiter}");

            public IniDocumentBuilder AppendPropertyLine(PropertyKey propertyKey, PropertyValue propertyValue)
                => this.AppendLine($"{propertyKey}{this.configuration.PropertyAssignmentSpacer}{this.configuration.PropertyAssignmentSeparator}{this.configuration.PropertyAssignmentSpacer}{propertyValue}");

            private IniDocumentBuilder AppendLine(string line)
            {
                this.stringBuilder.Append($"{line}{this.configuration.NewlineString}");

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
