namespace Delizious.Ini.Test
{
    using System;
    using System.Collections.Immutable;
    using System.IO;

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

                var content = $"[A]{Environment.NewLine}[B]{Environment.NewLine}[C]{Environment.NewLine}";
                using var stringReader = new StringReader(content);

                var target = IniDocument.LoadFrom(stringReader);

                var actual = target.SectionNames().ToImmutableArray();

                CollectionAssert.AreEqual(expected, actual);
            }
        }
    }
}
