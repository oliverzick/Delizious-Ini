using System;
using System.IO;
using Delizious.Ini;

Run(QuickStart, nameof(QuickStart));

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

void Run(Action action, string name)
{
    WriteHeadline(name);
    action();
}

void WriteHeadline(string headline)
    => Console.WriteLine(@"{0}{1}{0}{2}{0}{1}{0}", Environment.NewLine, new string('=', 80), headline);
