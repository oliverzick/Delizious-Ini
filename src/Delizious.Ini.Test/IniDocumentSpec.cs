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
                var expected = ImmutableArray.Create<SectionName>("A", "B", "C");

                var target = MakeTarget(expected);

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
            public void Throws_section_not_found_exception_when_section_specified_by_given_section_name_does_not_exist()
            {
                var sectionName = "NonExistingSection";
                var expected = new SectionNotFoundExceptionAssertion(sectionName);

                var target = MakeEmptyTarget();

                var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.PropertyKeys("NonExistingSection"));

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void Provides_property_keys_for_given_section_name()
            {
                var sectionName = "Section";
                var propertyKeys = ImmutableArray.Create<PropertyKey>("PropertyA", "PropertyB", "PropertyC");
                var expected = propertyKeys;

                var target = MakeTarget(Section.Create(sectionName, propertyKeys.Select(Property.Create)));

                var actual = target.PropertyKeys(sectionName).ToImmutableArray();

                CollectionAssert.AreEqual(expected, actual);
            }
        }

        private static IniDocument MakeEmptyTarget()
            => new IniDocumentBuilder().Build();

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

            public IniDocumentBuilder AppendPropertyLine(PropertyKey propertyKey, string propertyValue)
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
            public static Section Create(SectionName SectionName, IEnumerable<Property> properties)
                => new(SectionName, properties.ToImmutableArray());

            public IniDocumentBuilder ApplyTo(IniDocumentBuilder builder)
                => this.Properties.Aggregate(builder.AppendSectionLine(this.SectionName), (current, property) => property.ApplyTo(current));
        }

        private sealed record Property(PropertyKey PropertyKey, string PropertyValue)
        {
            public static Property Create(PropertyKey propertyKey)
                => new(propertyKey, "Default");

            public IniDocumentBuilder ApplyTo(IniDocumentBuilder builder)
                => builder.AppendPropertyLine(this.PropertyKey, this.PropertyValue);
        }
    }
}
