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
            public void Throws_ini_exception_when_text_reader_is_closed()
            {
                using var textReader = new StringReader(string.Empty);
                textReader.Close();

                Assert.ThrowsException<IniException>(() => IniDocument.LoadFrom(textReader));
            }
        }

        [TestClass]
        public sealed class SectionNames
        {
            [TestMethod]
            public void Provides_section_names()
            {
                var sectionNames = ImmutableArray.Create<SectionName>("A", "B", "C");
                var expected = sectionNames;

                var target = MakeTarget(sectionNames);

                var actual = target.SectionNames().ToImmutableArray();

                CollectionAssert.AreEqual(expected, actual);
            }

            private static IniDocument MakeTarget(IEnumerable<SectionName> sectionNames)
                => sectionNames.Aggregate(new IniDocumentBuilder(), (builder, sectionName) => builder.AppendSectionLine(sectionName)).Build();
        }

        [TestClass]
        public sealed class PropertyKeys
        {
            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = MakeEmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.PropertyKeys(null));
            }

            [TestMethod]
            public void Throws_section_not_found_exception_when_section_specified_by_its_section_name_does_not_exist()
            {
                var sectionName = NonexistentSectionName;
                var expected = new SectionNotFoundExceptionAssertion(sectionName);

                var target = MakeEmptyTarget();

                var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.PropertyKeys(sectionName));

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void Provides_property_keys_for_section_specified_by_its_section_name()
            {
                var sectionName = DefaultSectionName;
                var propertyKeys = ImmutableArray.Create<PropertyKey>("PropertyA", "PropertyB", "PropertyC");
                var expected = propertyKeys;

                var target = MakeTarget(Section.Create(sectionName, propertyKeys.Select(Property.Create)));

                var actual = target.PropertyKeys(sectionName).ToImmutableArray();

                CollectionAssert.AreEqual(expected, actual);
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
                const string sectionName = NonexistentSectionName;
                var expected = new SectionNotFoundExceptionAssertion(sectionName);

                var target = MakeEmptyTarget();

                var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.ReadPropertyValue(sectionName, Dummy));

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void Throws_property_not_found_exception_when_property_specified_by_its_property_key_does_not_exist()
            {
                const string sectionName = DefaultSectionName;
                const string propertyKey = NonexistentPropertyKey;
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
                const string sectionName = NonexistentSectionName;
                var expected = new SectionNotFoundExceptionAssertion(sectionName);

                var target = MakeEmptyTarget();

                var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.UpdatePropertyValue(sectionName, Dummy, Dummy));

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void Throws_property_not_found_exception_when_property_specified_by_its_property_key_does_not_exist()
            {
                const string sectionName = DefaultSectionName;
                const string propertyKey = NonexistentPropertyKey;
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

        private static IniDocument MakeTarget(params Section[] sections)
            => sections.Aggregate(new IniDocumentBuilder(), (builder, section) => section.ApplyTo(builder)).Build();

        private sealed class IniDocumentBuilder
        {
            private readonly StringBuilder stringBuilder = new();

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
