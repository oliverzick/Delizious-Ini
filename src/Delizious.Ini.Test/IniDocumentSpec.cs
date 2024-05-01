namespace Delizious.Ini.Test
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.IO;
    using System.Linq;
    using System.Text;

    [TestClass]
    public sealed class IniDocumentSpec
    {
        private const string Dummy = "Dummy";
        private const string NonexistentSectionName = "NonexistentSection";
        private const string NonexistentPropertyKey = "NonexistentProperty";

        private const string DefaultSectionName = "Section";
        private const string DefaultPropertyKey = "Property";

        [TestClass]
        public sealed class LoadFrom
        {
            [TestMethod]
            public void Throws_argument_null_exception_when_text_reader_is_null()
            {
                Assert.ThrowsException<ArgumentNullException>(() => IniDocument.LoadFrom(null));
            }

            [TestMethod]
            public void Throws_persistence_exception_containing_object_disposed_exception_when_text_reader_is_already_disposed()
            {
                var expected = PersistenceExceptionAssertion.Create<ObjectDisposedException>();
                using var textReader = new StringReader(string.Empty);
                textReader.Close();

                var actual = Assert.ThrowsException<PersistenceException>(() => IniDocument.LoadFrom(textReader));

                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public sealed class SaveTo
        {
            [TestMethod]
            public void Throws_argument_null_exception_when_text_writer_is_null()
            {
                var target = MakeEmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.SaveTo(null));
            }

            [TestMethod]
            public void Throws_persistence_exception_containing_object_disposed_exception_when_text_writer_is_already_disposed()
            {
                var expected = PersistenceExceptionAssertion.Create<ObjectDisposedException>();
                using var textWriter = new StringWriter();
                textWriter.Dispose();

                var target = MakeEmptyTarget();

                var actual = Assert.ThrowsException<PersistenceException>(() => target.SaveTo(textWriter));

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void Saves_the_ini_document_to_text_writer()
            {
                var expected = MakeSampleString();
                var stringBuilder = new StringBuilder();
                using var textWriter = new StringWriter(stringBuilder);

                var target = MakeSampleTarget();

                target.SaveTo(textWriter);
                textWriter.Flush();

                var actual = stringBuilder.ToString();

                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public sealed class EnumerateSections
        {
            [TestMethod]
            public void Enumerates_the_names_of_all_contained_sections()
            {
                var sectionNames = ImmutableArray.Create<SectionName>("A", "B", "C");
                var expected = sectionNames;

                var target = MakeTarget(sectionNames);

                var actual = target.EnumerateSections().ToImmutableArray();

                CollectionAssert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public sealed class EnumerateProperties
        {
            [TestClass]
            public sealed class With_sectionName
            {
                [TestMethod]
                public void Throws_argument_null_exception_when_section_name_is_null()
                {
                    var target = MakeEmptyTarget();

                    Assert.ThrowsException<ArgumentNullException>(() => target.EnumerateProperties(null));
                }

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_specified_by_its_section_name_does_not_exist()
                {
                    var sectionName = NonexistentSectionName;
                    var expected = new SectionNotFoundExceptionAssertion(sectionName);

                    var target = MakeEmptyTarget();

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.EnumerateProperties(sectionName));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Enumerates_the_keys_of_all_properties_contained_in_the_specified_section()
                {
                    var sectionName = DefaultSectionName;
                    var propertyKeys = ImmutableArray.Create<PropertyKey>("PropertyA", "PropertyB", "PropertyC");
                    var expected = propertyKeys;

                    var target = MakeTarget(Section.Create(sectionName, propertyKeys.Select(Property.Create)));

                    var actual = target.EnumerateProperties(sectionName).ToImmutableArray();

                    CollectionAssert.AreEqual(expected, actual);
                }
            }
        }

        [TestClass]
        public sealed class ReadPropertyValue
        {
            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = MakeEmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadPropertyValue(null, Dummy));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = MakeEmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.ReadPropertyValue(Dummy, null));
            }

            [TestMethod]
            public void Throws_section_not_found_exception_when_section_specified_by_its_section_name_does_not_exist()
            {
                var sectionName = NonexistentSectionName;
                var expected = new SectionNotFoundExceptionAssertion(sectionName);

                var target = MakeEmptyTarget();

                var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.ReadPropertyValue(sectionName, Dummy));

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void Throws_property_not_found_exception_when_property_specified_by_its_property_key_does_not_exist()
            {
                var sectionName = DefaultSectionName;
                var propertyKey = NonexistentPropertyKey;
                var expected = new PropertyNotFoundExceptionAssertion(propertyKey);

                var target = MakeTarget(Section.Create(sectionName));

                var actual = Assert.ThrowsException<PropertyNotFoundException>(() => target.ReadPropertyValue(sectionName, propertyKey));

                Assert.AreEqual(expected, actual);
            }

            [DataTestMethod]
            [DataRow("Property value", DisplayName = "Actual value")]
            [DataRow(""              , DisplayName = "Empty string when property does exist but has no value")]
            public void Provides_property_value_for_property_with_specified_section_name_and_property_key(string propertyValue)
            {
                var expected = propertyValue;

                var target = MakeSinglePropertyTarget(expected);

                var actual = target.ReadPropertyValue(DefaultSectionName, DefaultPropertyKey);

                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public sealed class UpdatePropertyValue
        {
            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = MakeEmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.UpdatePropertyValue(null, Dummy, Dummy));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = MakeEmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.UpdatePropertyValue(Dummy, null, Dummy));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_new_property_value_is_null()
            {
                var target = MakeEmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.UpdatePropertyValue(Dummy, Dummy, null));
            }

            [TestMethod]
            public void Throws_section_not_found_exception_when_section_specified_by_its_section_name_does_not_exist()
            {
                var sectionName = NonexistentSectionName;
                var expected = new SectionNotFoundExceptionAssertion(sectionName);

                var target = MakeEmptyTarget();

                var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.UpdatePropertyValue(sectionName, Dummy, Dummy));

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void Throws_property_not_found_exception_when_property_specified_by_its_property_key_does_not_exist()
            {
                var sectionName = DefaultSectionName;
                var propertyKey = NonexistentPropertyKey;
                var expected = new PropertyNotFoundExceptionAssertion(propertyKey);

                var target = MakeTarget(Section.Create(sectionName));

                var actual = Assert.ThrowsException<PropertyNotFoundException>(() => target.UpdatePropertyValue(sectionName, propertyKey, Dummy));

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void Updates_property_value_to_new_value_for_property_with_specified_section_name_and_property_key()
            {
                var sectionName = DefaultSectionName;
                var propertyKey = DefaultPropertyKey;
                var oldValue = "Old value";
                var newValue = "New value";
                var expected = newValue;

                var target = MakeSinglePropertyTarget(oldValue);

                target.UpdatePropertyValue(sectionName, propertyKey, newValue);

                var actual = target.ReadPropertyValue(sectionName, propertyKey);

                Assert.AreEqual(expected, actual);
            }
        }

        private static IniDocument MakeEmptyTarget()
            => new IniDocumentBuilder().Build();

        private static IniDocument MakeSinglePropertyTarget(PropertyValue propertyValue)
            => MakeTarget(Section.Create(DefaultSectionName, Property.Create(DefaultPropertyKey, propertyValue)));

        private static IniDocument MakeTarget(IEnumerable<SectionName> sectionNames)
            => MakeTarget(sectionNames.Select(sectionName => Section.Create(sectionName)).ToArray());

        private static IniDocument MakeTarget(params Section[] sections)
            => sections.Aggregate(new IniDocumentBuilder(), (builder, section) => section.ApplyTo(builder)).Build();

        private static IniDocument MakeSampleTarget()
            => MakeTarget(Section.Create("Section1", Property.Create("PropertyA", "Value A")),
                          Section.Create("Section2", Property.Create("PropertyB", "Value B")));

        public static string MakeSampleString()
            => new IniDocumentBuilder().AppendSectionLine("Section1")
                                       .AppendPropertyLine("PropertyA", "Value A")
                                       .AppendEmptyLine()
                                       .AppendSectionLine("Section2")
                                       .AppendPropertyLine("PropertyB", "Value B")
                                       .ToString();

        private sealed class IniDocumentBuilder
        {
            private readonly StringBuilder stringBuilder = new();

            public IniDocumentBuilder AppendEmptyLine()
            {
                this.stringBuilder.AppendLine();

                return this;
            }

            public IniDocumentBuilder AppendSectionLine(SectionName sectionName)
            {
                this.stringBuilder.AppendLine($"[{sectionName}]");

                return this;
            }

            public IniDocumentBuilder AppendPropertyLine(PropertyKey propertyKey, PropertyValue propertyValue)
            {
                this.stringBuilder.AppendLine($"{propertyKey}={propertyValue}");

                return this;
            }

            public override string ToString()
                => this.stringBuilder.ToString();

            public IniDocument Build()
            {
                using var stringReader = new StringReader(this.ToString());
                return IniDocument.LoadFrom(stringReader);
            }
        }

        private sealed record Section(SectionName SectionName, ImmutableArray<Property> Properties)
        {
            public static Section Create(SectionName sectionName, params Property[] properties)
                => Create(sectionName, properties.AsEnumerable());

            public static Section Create(SectionName SectionName, IEnumerable<Property> properties)
                => new(SectionName, properties.ToImmutableArray());

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
