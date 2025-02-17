using System;
using System.IO;
using Delizious.Ini;

Run(QuickStart,          nameof(QuickStart));
Run(Configure,           nameof(Configure));
Run(LoadAndSave,         nameof(LoadAndSave));
Run(EnumerateSections,   nameof(EnumerateSections));
Run(EnumerateProperties, nameof(EnumerateProperties));
Run(DeleteSection,       nameof(DeleteSection));
Run(DeleteProperty,      nameof(DeleteProperty));

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
    var configuration = IniDocumentConfiguration.Default
                                                .WithCaseSensitivity(CaseSensitivity.CaseSensitive); // Treat section names and property keys as case-sensitive (by default, case-insensitive)

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

void Configure()
{
    // This configuration represents the loose configuration which is also predefined:
    //var looseConfiguration = IniDocumentConfiguration.Loose;
    var looseConfiguration =
        IniDocumentConfiguration.Default
                                .WithCaseSensitivity(CaseSensitivity.CaseInsensitive) // Treat section names and property keys as case-insensitive
                                .WithNewlineString(NewlineString.Environment) // Use newline string as given by current environment
                                .WithSectionBeginningDelimiter(SectionBeginningDelimiter.Default) // Use default section beginning delimiter which is opening square bracket '['
                                .WithSectionEndDelimiter(SectionEndDelimiter.Default) // Use default section end delimiter which is closing square bracket ']'
                                .WithSectionNameRegex(SectionNameRegex.Default) // Use default section name regex which is '[\p{L}\p{M}\p{N}\p{P}\p{S}\p{Zs}]+'
                                .WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Ignore) // Ignore subsequent occurrences of a duplicate property by using the first occurrence of such a property
                                .WithDuplicateSectionBehavior(DuplicateSectionBehavior.Merge) // Merge a duplicate section
                                .WithInvalidLineBehavior(InvalidLineBehavior.Ignore) // Ignore when a line is invalid and cannot be parsed on loading
                                .WithPropertyAssignmentSeparator(PropertyAssignmentSeparator.Default) // Use default property assignment separator which is equality sign '='
                                .WithPropertyAssignmentSpacer(PropertyAssignmentSpacer.None) // Use no property assignment spacer
                                .WithPropertyEnumerationMode(PropertyEnumerationMode.Fallback) // Fallback to empty collection of property keys when section does not exist
                                .WithPropertyReadMode(PropertyReadMode.Fallback) // Fallback to empty string when property to read does not exist
                                .WithPropertyWriteMode(PropertyWriteMode.Create) // Create a new property or update an existing property
                                .WithPropertyDeletionMode(PropertyDeletionMode.Ignore) // Ignore when property to delete does not exist
                                .WithSectionDeletionMode(SectionDeletionMode.Ignore); // Ignore when section to delete does not exist

    // This configuration represents the strict configuration which is also predefined:
    //var strictConfiguration = IniDocumentConfiguration.Strict;
    var strictConfiguration =
        IniDocumentConfiguration.Default
                                .WithCaseSensitivity(CaseSensitivity.CaseInsensitive) // Treat section names and property keys as case-insensitive
                                .WithNewlineString(NewlineString.Environment) // Use newline string as given by current environment
                                .WithSectionBeginningDelimiter(SectionBeginningDelimiter.Default) // Use default section beginning delimiter which is opening square bracket '['
                                .WithSectionEndDelimiter(SectionEndDelimiter.Default) // Use default section end delimiter which is closing square bracket ']'
                                .WithSectionNameRegex(SectionNameRegex.Default) // Use default section name regex which is '[\p{L}\p{M}\p{N}\p{P}\p{S}\p{Zs}]+'
                                .WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Fail) // Throw exception when a duplicate property occurs
                                .WithDuplicateSectionBehavior(DuplicateSectionBehavior.Fail) // Throw exception when a duplicate section occurs
                                .WithInvalidLineBehavior(InvalidLineBehavior.Fail) // Throw exception when a line is invalid and cannot be parsed on loading
                                .WithPropertyAssignmentSeparator(PropertyAssignmentSeparator.Default) // Use default property assignment separator which is equality sign '='
                                .WithPropertyAssignmentSpacer(PropertyAssignmentSpacer.None) // Use no property assignment spacer
                                .WithPropertyEnumerationMode(PropertyEnumerationMode.Fail) // Throw exception when section to enumerate properties does not exist
                                .WithPropertyReadMode(PropertyReadMode.Fail) // Throw exception when property to read to does not exist
                                .WithPropertyWriteMode(PropertyWriteMode.Update) // Update existing property only but throw exception when property to write does not exist
                                .WithPropertyDeletionMode(PropertyDeletionMode.Fail) // Throw exception when property to delete does not exist
                                .WithSectionDeletionMode(SectionDeletionMode.Fail); // Throw exception when section to delete does not exist
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

void EnumerateProperties()
{
    const string ini = """
                       [Section]
                       Property=Current value
                       AnotherProperty=Another value
                       EmptyProperty=
                       """;

    using var textReader = new StringReader(ini);
    var iniDocument = IniDocument.LoadFrom(textReader, IniDocumentConfiguration.Default);

    foreach (var propertyName in iniDocument.EnumerateProperties("Section"))
    {
        Console.WriteLine(propertyName);
    }
}

void DeleteSection()
{
    const string ini = """
                       [Section]
                       Property=Current value

                       [EmptySection]

                       [AnotherSection]
                       AnotherProperty=With another value
                       """;

    using var textReader = new StringReader(ini);
    var iniDocument = IniDocument.LoadFrom(textReader, IniDocumentConfiguration.Default);

    iniDocument.DeleteSection("EmptySection");

    iniDocument.SaveTo(Console.Out);
}

void DeleteProperty()
{
    const string ini = """
                       [Section]
                       Property=Current value
                       AnotherProperty=Another value
                       EmptyProperty=
                       """;

    using var textReader = new StringReader(ini);
    var iniDocument = IniDocument.LoadFrom(textReader, IniDocumentConfiguration.Default);

    iniDocument.DeleteProperty("Section", "Property");

    iniDocument.SaveTo(Console.Out);
}

void Run(Action action, string name)
{
    WriteHeadline(name);
    action();
}

void WriteHeadline(string headline)
    => Console.WriteLine(@"{0}{1}{0}{2}{0}{1}{0}", Environment.NewLine, new string('=', 80), headline);
