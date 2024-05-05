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
        private static readonly SectionName DummySectionName = "Dummy";
        private static readonly PropertyKey DummyPropertyKey = "Dummy";
        private static readonly PropertyValue DummyPropertyValue = "Dummy";

        private static readonly SectionName NonexistentSectionName = "NonexistentSection";
        private static readonly PropertyKey NonexistentPropertyKey = "NonexistentProperty";

        private static readonly SectionName DefaultSectionName = "Section";
        private static readonly PropertyKey DefaultPropertyKey = "Property";
        private static readonly PropertyValue DefaultPropertyValue = "Value";

        private static readonly PropertyValue EmptyPropertyValue = string.Empty;

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

            [TestMethod]
            public void Saves_the_ini_document_to_text_writer()
            {
                var expected = Make.SampleString();
                var stringBuilder = new StringBuilder();
                using var textWriter = new StringWriter(stringBuilder);

                var target = Make.SampleTarget();

                target.SaveTo(textWriter);
                textWriter.Flush();

                var actual = stringBuilder.ToString();

                Assert.AreEqual(expected, actual);
            }
        }

        [TestClass]
        public sealed class EnumerateSections
        {
            private static ImmutableArray<SectionName> SectionNames => ImmutableArray.Create<SectionName>("Section1", "AnotherSection2", "SomeSectionA");

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
            private static ImmutableArray<PropertyKey> PropertyKeys { get; } = ImmutableArray.Create<PropertyKey>("Property1", "AnotherProperty2", "SomePropertyA");

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
                public void Throws_section_not_found_exception_when_section_specified_by_its_section_name_does_not_exist()
                {
                    var sectionName = NonexistentSectionName;
                    var expected = new SectionNotFoundExceptionAssertion(sectionName);

                    var target = Make.EmptyTarget();

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.EnumerateProperties(sectionName));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Enumerates_the_keys_of_all_properties_contained_in_the_specified_section()
                {
                    var expected = PropertyKeys;
                    var target = Make.SingleDefaultSectionTarget(expected);

                    var actual = target.EnumerateProperties(DefaultSectionName).ToImmutableArray();

                    CollectionAssert.AreEqual(expected, actual);
                }
            }

            [TestClass]
            public sealed class With_sectionName_and_mode
            {
                private static readonly PropertyEnumerationMode DummyMode = PropertyEnumerationMode.Fail();

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
                    yield return new object[] { PropertyEnumerationMode.Fail() };
                    yield return new object[] { PropertyEnumerationMode.Fallback() };
                }

                [TestClass]
                public sealed class When_fail_mode
                {
                    private static readonly PropertyEnumerationMode Mode = PropertyEnumerationMode.Fail();

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
                    private static readonly PropertyEnumerationMode Mode = PropertyEnumerationMode.Fallback();

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

                [TestMethod]
                public void Throws_section_not_found_exception_when_section_does_not_exist()
                {
                    var sectionName = NonexistentSectionName;
                    var expected = new SectionNotFoundExceptionAssertion(sectionName);

                    var target = Make.EmptyTarget();

                    var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.ReadProperty(sectionName, DummyPropertyKey));

                    Assert.AreEqual(expected, actual);
                }

                [TestMethod]
                public void Throws_property_not_found_exception_when_property_does_not_exist()
                {
                    var sectionName = DefaultSectionName;
                    var propertyKey = NonexistentPropertyKey;
                    var expected = new PropertyNotFoundExceptionAssertion(propertyKey);

                    var target = Make.SingleDefaultPropertyTarget(DefaultPropertyValue);

                    var actual = Assert.ThrowsException<PropertyNotFoundException>(() => target.ReadProperty(sectionName, propertyKey));

                    Assert.AreEqual(expected, actual);
                }

                [DataTestMethod]
                [DataRow("Property value", DisplayName = "Actual value")]
                [DataRow(""              , DisplayName = "Empty string when property does exist but has no value")]
                public void Reads_the_value_of_the_property_contained_in_the_specified_section(string propertyValue)
                {
                    var expected = propertyValue;

                    var target = Make.SingleDefaultPropertyTarget(expected);

                    var actual = target.ReadProperty(DefaultSectionName, DefaultPropertyKey);

                    Assert.AreEqual(expected, actual);
                }
            }

            [TestClass]
            public sealed class With_sectionName_and_propertyKey_and_mode
            {
                private static PropertyReadMode DummyMode => PropertyReadMode.Fail();

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
                    yield return new object[] { EmptyPropertyValue, PropertyReadMode.Fallback() };
                    yield return new object[] { EmptyPropertyValue, PropertyReadMode.Fail() };

                    yield return new object[] { DefaultPropertyValue, PropertyReadMode.Fallback() };
                    yield return new object[] { DefaultPropertyValue, PropertyReadMode.Fail() };
                }

                [TestClass]
                public sealed class When_fail_mode
                {
                    private static readonly PropertyReadMode Mode = PropertyReadMode.Fail();

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

                    private static PropertyReadMode Mode => PropertyReadMode.Fallback(FallbackPropertyValue);

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
        public sealed class UpdateProperty
        {
            [TestMethod]
            public void Throws_argument_null_exception_when_section_name_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.UpdateProperty(null, DummyPropertyKey, DummyPropertyValue));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_property_key_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.UpdateProperty(DummySectionName, null, DummyPropertyValue));
            }

            [TestMethod]
            public void Throws_argument_null_exception_when_new_property_value_is_null()
            {
                var target = Make.EmptyTarget();

                Assert.ThrowsException<ArgumentNullException>(() => target.UpdateProperty(DummySectionName, DummyPropertyKey, null));
            }

            [TestMethod]
            public void Throws_section_not_found_exception_when_section_does_not_exist()
            {
                var sectionName = NonexistentSectionName;
                var expected = new SectionNotFoundExceptionAssertion(sectionName);

                var target = Make.EmptyTarget();

                var actual = Assert.ThrowsException<SectionNotFoundException>(() => target.UpdateProperty(sectionName, DummyPropertyKey, DummyPropertyValue));

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void Throws_property_not_found_exception_when_property_does_not_exist()
            {
                var sectionName = DefaultSectionName;
                var propertyKey = NonexistentPropertyKey;
                var expected = new PropertyNotFoundExceptionAssertion(propertyKey);

                var target = Make.SingleDefaultPropertyTarget(DefaultPropertyValue);

                var actual = Assert.ThrowsException<PropertyNotFoundException>(() => target.UpdateProperty(sectionName, propertyKey, DummyPropertyValue));

                Assert.AreEqual(expected, actual);
            }

            [TestMethod]
            public void Updates_the_value_of_the_property_contained_in_the_section()
            {
                var sectionName = DefaultSectionName;
                var propertyKey = DefaultPropertyKey;
                var oldValue = "Old value";
                var newValue = "New value";
                var expected = newValue;

                var target = Make.SingleDefaultPropertyTarget(oldValue);

                target.UpdateProperty(sectionName, propertyKey, newValue);

                var actual = target.ReadProperty(sectionName, propertyKey);

                Assert.AreEqual(expected, actual);
            }
        }

        private static class Make
        {
            public static IniDocument EmptyTarget()
                => new IniDocumentBuilder().Build();

            public static IniDocument SingleDefaultSectionTarget(IEnumerable<PropertyKey> propertyKeys)
                => Target(Section.Create(DefaultSectionName, propertyKeys.Select(Property.Create)));

            public static IniDocument SingleDefaultPropertyTarget(PropertyValue propertyValue)
                => Target(Section.Create(DefaultSectionName, Property.Create(DefaultPropertyKey, propertyValue)));

            public static IniDocument EmptySectionsTarget(IEnumerable<SectionName> sectionNames)
                => Target(sectionNames.Select(Section.CreateEmpty));

            public static IniDocument SampleTarget()
                => Target(Section.Create("Section1", Property.Create("PropertyA", "Value A")),
                          Section.Create("Section2", Property.Create("PropertyB", "Value B")));

            public static string SampleString()
                => new IniDocumentBuilder().AppendSectionLine("Section1")
                                           .AppendPropertyLine("PropertyA", "Value A")
                                           .AppendEmptyLine()
                                           .AppendSectionLine("Section2")
                                           .AppendPropertyLine("PropertyB", "Value B")
                                           .ToString();

            private static IniDocument Target(params Section[] sections)
                => Target(sections.AsEnumerable());

            private static IniDocument Target(IEnumerable<Section> sections)
                => sections.Aggregate(new IniDocumentBuilder(), (builder, section) => section.ApplyTo(builder)).Build();

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
                public static Section CreateEmpty(SectionName sectionName)
                    => Create(sectionName);

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
}
