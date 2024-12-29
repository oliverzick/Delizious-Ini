# Delizious Ini
## What?
Delizious Ini is an easy to use .NET Standard library entirely written in C# for reading and writing of INI data.

## Features
Delizious Ini provides the following features:
* Intuitive API design applying [Domain-driven design (DDD)](https://en.wikipedia.org/wiki/Domain-driven_design)
* Enumeration of sections
* Enumeration of properties contained in a section
* Reading and writing of a property
* Deletion of sections or properties
* Configurability of the failure behavior (e.g. throw a specific exception in case a section or property does not exist, or proceed with a fallback behavior) for almost every operation on instance and operation level
* Configurability of case sensitivity that specifies how to treat section names and property keys

Upcoming features:
* Configurability of behavior in case of invalid lines
* Configurability of behavior whether to allow duplicated sections
* Configurability of behavior whether to allow duplicated keys
* Configurability of the property's assignment character and its spacer
* Configurability of the new line string
* Support for comments
* Merge two INI documents
* ...

## Getting started
To install Delizious Ini, run the following command in the respective console:

### Package Manager Console
    PM> Install-Package Delizious.Ini

### .NET CLI Console
    > dotnet add package Delizious.Ini

### Quick start
```cs
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
```

## Examples
### Configure default behavior of an INI document
```cs
// This configuration represents the loose configuration which is also predefined:
//var looseConfiguration = IniDocumentConfiguration.Loose;
var looseConfiguration =
    IniDocumentConfiguration.Default
                            .WithCaseSensitivity(CaseSensitivity.CaseInsensitive) // Treat section names and property keys as case-insensitive
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
                            .WithPropertyEnumerationMode(PropertyEnumerationMode.Fail) // Throw exception when section to enumerate properties does not exist
                            .WithPropertyReadMode(PropertyReadMode.Fail) // Throw exception when property to read to does not exist
                            .WithPropertyWriteMode(PropertyWriteMode.Update) // Update existing property only but throw exception when property to write does not exist
                            .WithPropertyDeletionMode(PropertyDeletionMode.Fail) // Throw exception when property to delete does not exist
                            .WithSectionDeletionMode(SectionDeletionMode.Fail); // Throw exception when section to delete does not exist
```

### Load and save
```cs
const string ini = """
                   [Section]
                   Property=Current value
                   """;

using var textReader = new StringReader(ini);
var iniDocument = IniDocument.LoadFrom(textReader, IniDocumentConfiguration.Default);

// Save entire INI document to text writer by using Console.Out to output content
var textWriter = Console.Out;
iniDocument.SaveTo(textWriter);
```

### Enumerate sections
```cs
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
```

### Enumerate properties
```cs
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
```

### Delete section
```cs
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
```

### Delete property
```cs
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
```

## License
MIT License

[https://opensource.org/license/mit](https://opensource.org/license/mit)

## Socialize
If you like or use my work and you are interested in this kind of software development let's get in touch. :)
