using System;
using System.IO;
using Delizious.Ini;

Run(QuickStart,        nameof(QuickStart));
Run(LoadAndSave,       nameof(LoadAndSave));
Run(EnumerateSections, nameof(EnumerateSections));

return;

void QuickStart()
{
    const string ini = """
                       [Section]
                       Property=Current value
                       AnotherProperty=Another value

                       [EmptySection]
                       """;

    using var textReader = new StringReader(ini);

    // Use default configuration 
    var configuration = IniDocumentConfiguration.Default;
    var iniDocument = IniDocument.LoadFrom(textReader, configuration);

    // Read existing property
    var originalValue = iniDocument.ReadProperty("Section", "Property");
    Console.WriteLine($@"Original property value: {originalValue}");

    // Update existing property
    iniDocument.WriteProperty("Section", "Property", "This is the new value");

    var updatedValue = iniDocument.ReadProperty("Section", "Property");
    Console.WriteLine($@"Updated property value: {updatedValue}");

    // Write new property
    iniDocument.WriteProperty("NewSection", "NewProperty", "NewValue");

    // Delete section
    iniDocument.DeleteSection("EmptySection");

    // Delete property
    iniDocument.DeleteProperty("Section", "AnotherProperty");

    Console.WriteLine();
    Console.WriteLine(@"INI document:");
    iniDocument.SaveTo(Console.Out);
}

void LoadAndSave()
{
    const string ini = """
                       [Section]
                       Property=Current value
                       """;

    using var textReader = new StringReader(ini);
    var iniDocument = IniDocument.LoadFrom(textReader, IniDocumentConfiguration.Default);

    // Save entire INI document to text writer by using Console.Out to output content
    var textWriter = Console.Out;
    iniDocument.SaveTo(textWriter);
}

void EnumerateSections()
{
    const string ini = """
                       [Section]
                       Property=Current value

                       [EmptySection]
                       """;

    using var textReader = new StringReader(ini);
    var iniDocument = IniDocument.LoadFrom(textReader, IniDocumentConfiguration.Default);

    foreach (var sectionName in iniDocument.EnumerateSections())
    {
        Console.WriteLine(sectionName);
    }
}

void Run(Action action, string name)
{
    WriteHeadline(name);
    action();
}

void WriteHeadline(string headline)
    => Console.WriteLine(@"{0}{1}{0}{2}{0}{1}{0}", Environment.NewLine, new string('=', 80), headline);
