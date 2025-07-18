# Delizious Ini
## What?
Delizious Ini is an easy to use .NET Standard library entirely written in C# for reading and writing of INI data.

## New features in version 1.20.0
* Document available modes `PropertyEnumerationMode`, `PropertyReadMode`, `PropertyWriteMode`, `SectionDeletionMode`, `PropertyDeletionMode`, `CommentReadMode` and `CommentWriteMode` in README
* Improve readability of [Features](#features) section in README

## Features
Delizious Ini provides the following features:
* Intuitive API design applying [Domain-driven design (DDD)](https://en.wikipedia.org/wiki/Domain-driven_design)
* [Enumeration of sections](#enumeration-of-sections)
* [Enumeration of properties](#enumeration-of-properties)
* [Reading of a property](#reading-of-a-property)
* [Writing of a property](#writing-of-a-property)
* [Deletion of a section](#deletion-of-a-section)
* [Deletion of a property](#deletion-of-a-property)
* [Reading the comment of a section](#reading-the-comment-of-a-section)
* [Reading the comment of a property](#reading-the-comment-of-a-property)
* [Writing the comment of a section](#writing-the-comment-of-a-section)
* [Writing the comment of a property](#writing-the-comment-of-a-property)
* Cloning an INI document
* [Configurability](#configure-default-behavior-of-an-ini-document)
  * Failure behavior (e.g. throw a specific exception in case a section or property does not exist, or proceed with a fallback behavior) for almost every operation on instance and operation level
  * Case sensitivity that specifies how to treat section names and property keys
  * Invalid line behavior (`Fail` or `Ignore`)
  * Property's assignment separator and assignment spacer
  * Duplicate section behavior (`Fail` or `Merge`)
  * Duplicate property behavior (`Fail`, `Ignore` or `Override`)
  * Section's beginning and end delimiters
  * Regular expression pattern (regex) for section name
  * Newline string (`Environment`, `Windows` or `Unix`)
  * Comment string that indicates the beginning of a comment line

Upcoming features:
* Merge two INI documents

[&#8593;](#features)
---

## Getting started
To install Delizious Ini, run the following command in the respective console:

### Package Manager Console
    PM> Install-Package Delizious.Ini

### .NET CLI Console
    > dotnet add package Delizious.Ini

[&#8593;](#features)
---

## Quick start
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

[&#8593;](#features)
---

## Examples
### Configure default behavior of an INI document
```cs
// This configuration represents the loose configuration which is also predefined:
//var looseConfiguration = IniDocumentConfiguration.Loose;
var looseConfiguration =
    IniDocumentConfiguration.Default
                            .WithCaseSensitivity(CaseSensitivity.CaseInsensitive)                 // Treat section names and property keys as case-insensitive
                            .WithNewlineString(NewlineString.Environment)                         // Use newline string as given by current environment
                            .WithSectionBeginningDelimiter(SectionBeginningDelimiter.Default)     // Use default section beginning delimiter which is opening square bracket '['
                            .WithSectionEndDelimiter(SectionEndDelimiter.Default)                 // Use default section end delimiter which is closing square bracket ']'
                            .WithSectionNameRegex(SectionNameRegex.Default)                       // Use default section name regex which is '[\p{L}\p{M}\p{N}\p{P}\p{S}\p{Zs}]+'
                            .WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Ignore)      // Ignore subsequent occurrences of a duplicate property by using the first occurrence of such a property
                            .WithDuplicateSectionBehavior(DuplicateSectionBehavior.Merge)         // Merge a duplicate section
                            .WithInvalidLineBehavior(InvalidLineBehavior.Ignore)                  // Ignore when a line is invalid and cannot be parsed on loading
                            .WithPropertyAssignmentSeparator(PropertyAssignmentSeparator.Default) // Use default property assignment separator which is equality sign '='
                            .WithPropertyAssignmentSpacer(PropertyAssignmentSpacer.None)          // Use no property assignment spacer
                            .WithPropertyEnumerationMode(PropertyEnumerationMode.Fallback)        // Fallback to empty collection of property keys when section does not exist
                            .WithPropertyReadMode(PropertyReadMode.Fallback)                      // Fallback to empty string when property to read does not exist
                            .WithPropertyWriteMode(PropertyWriteMode.Create)                      // Create a new property or update an existing property
                            .WithPropertyDeletionMode(PropertyDeletionMode.Ignore)                // Ignore when property to delete does not exist
                            .WithSectionDeletionMode(SectionDeletionMode.Ignore)                  // Ignore when section to delete does not exist
                            .WithCommentString(CommentString.Default)                             // Use default comment string that indicates the beginning of a comment line which is a semicolon ';'
                            .WithCommentReadMode(CommentReadMode.Fallback)                        // Fallback to none comment when section or property to read comment does not exist
                            .WithCommentWriteMode(CommentWriteMode.Ignore);                       // Ignore when section or property to write the comment does not exist

// This configuration represents the strict configuration which is also predefined:
//var strictConfiguration = IniDocumentConfiguration.Strict;
var strictConfiguration =
    IniDocumentConfiguration.Default
                            .WithCaseSensitivity(CaseSensitivity.CaseInsensitive)                 // Treat section names and property keys as case-insensitive
                            .WithNewlineString(NewlineString.Environment)                         // Use newline string as given by current environment
                            .WithSectionBeginningDelimiter(SectionBeginningDelimiter.Default)     // Use default section beginning delimiter which is opening square bracket '['
                            .WithSectionEndDelimiter(SectionEndDelimiter.Default)                 // Use default section end delimiter which is closing square bracket ']'
                            .WithSectionNameRegex(SectionNameRegex.Default)                       // Use default section name regex which is '[\p{L}\p{M}\p{N}\p{P}\p{S}\p{Zs}]+'
                            .WithDuplicatePropertyBehavior(DuplicatePropertyBehavior.Fail)        // Throw exception when a duplicate property occurs
                            .WithDuplicateSectionBehavior(DuplicateSectionBehavior.Fail)          // Throw exception when a duplicate section occurs
                            .WithInvalidLineBehavior(InvalidLineBehavior.Fail)                    // Throw exception when a line is invalid and cannot be parsed on loading
                            .WithPropertyAssignmentSeparator(PropertyAssignmentSeparator.Default) // Use default property assignment separator which is equality sign '='
                            .WithPropertyAssignmentSpacer(PropertyAssignmentSpacer.None)          // Use no property assignment spacer
                            .WithPropertyEnumerationMode(PropertyEnumerationMode.Fail)            // Throw exception when section to enumerate properties does not exist
                            .WithPropertyReadMode(PropertyReadMode.Fail)                          // Throw exception when property to read to does not exist
                            .WithPropertyWriteMode(PropertyWriteMode.Update)                      // Update existing property only but throw exception when property to write does not exist
                            .WithPropertyDeletionMode(PropertyDeletionMode.Fail)                  // Throw exception when property to delete does not exist
                            .WithSectionDeletionMode(SectionDeletionMode.Fail)                    // Throw exception when section to delete does not exist
                            .WithCommentString(CommentString.Default)                             // Use default comment string that indicates the beginning of a comment line which is a semicolon ';'
                            .WithCommentReadMode(CommentReadMode.Fail)                            // Throw exception when section or property to read comment does not exist
                            .WithCommentWriteMode(CommentWriteMode.Fail);                         // Throw exception when section or property to write the comment does not exist
```

[&#8593;](#features)
---

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

[&#8593;](#features)
---

### Enumeration of sections
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

[&#8593;](#features)
---

### Enumeration of properties
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

The enumeration of properties supports the following modes:

| Mode                               | Description |
|------------------------------------|-------------|
| `PropertyEnumerationMode.Fail`     | Throw a `SectionNotFoundException` when the section does not exist. |
| `PropertyEnumerationMode.Fallback` | Fall back to an empty collection of properties when the section does not exist. |

[&#8593;](#features)
---

### Reading of a property

The reading of a property supports the following modes:

| Mode                              | Description |
|-----------------------------------|-------------|
| `PropertyReadMode.Fail`           | Throw a `SectionNotFoundException` when the section does not exist, or throw a `PropertyNotFoundException` when the section exists but the property does not exist. |
| `PropertyReadMode.Fallback`       | Fall back to `PropertyValue.None` if the section or property does not exist. |
| `PropertyReadMode.CustomFallback` | Fall back to the given custom property value if the section or property does not exist. |

[&#8593;](#features)
---

### Writing of a property

The writing of a property supports the following modes:

| Mode                       | Description |
|----------------------------|-------------|
| `PropertyWriteMode.Create` | Create a new property. If the property already exists, it will be overwritten. If the section does not exist, a new section is created. If the section exists but the property itself does not exist, a new property is created. |
| `PropertyWriteMode.Update` | Update an existing property and require that both the section and property exist. Throw a `SectionNotFoundException` when the section does not exist, or throw a `PropertyNotFoundException` when the section exists but the property does not exist. |

[&#8593;](#features)
---

### Deletion of a section
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

The deletion of a section supports the following modes:

| Mode                         | Description |
|------------------------------|-------------|
| `SectionDeletionMode.Fail`   | Throw a `SectionNotFoundException` when the section does not exist. |
| `SectionDeletionMode.Ignore` | Silently ignore if the section does not exist. |

[&#8593;](#features)
---

### Deletion of a property
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

The deletion of a property supports the following modes:

| Mode                          | Description |
|-------------------------------|-------------|
| `PropertyDeletionMode.Fail`   | Throw a `SectionNotFoundException` when the section does not exist, or throw a `PropertyNotFoundException` when the section exists but the property does not exist. |
| `PropertyDeletionMode.Ignore` | Silently ignore if the section or the property does not exist. |

[&#8593;](#features)
---

### Reading the comment of a section
```cs
const string ini = """
                   ;This is a sample
                   ;multiline
                   ;comment. :)
                   [Section]
                   Property=Value
                   """;

using var textReader = new StringReader(ini);
var iniDocument = IniDocument.LoadFrom(textReader, IniDocumentConfiguration.Default);

var comment = iniDocument.ReadComment("Section");

Console.WriteLine(comment);
```

Reading the comment of a section supports the following modes:

| Mode                             | Description |
|----------------------------------|-------------|
| `CommentReadMode.Fail`           | Throw a `SectionNotFoundException` when the section does not exist. |
| `CommentReadMode.Fallback`       | Fall back to none comment if the section does not exist. |
| `CommentReadMode.CustomFallback` | Fall back to a custom fallback comment if the section does not exist. |

[&#8593;](#features)
---

### Reading the comment of a property
```cs
const string ini = """
                   [Section]
                   ;This is a sample
                   ;multiline
                   ;comment. :)
                   Property=Value
                   """;

using var textReader = new StringReader(ini);
var iniDocument = IniDocument.LoadFrom(textReader, IniDocumentConfiguration.Default);

var comment = iniDocument.ReadComment("Section", "Property");

Console.WriteLine(comment);
```

Reading the comment of a property supports the following modes:

| Mode                             | Description |
|----------------------------------|-------------|
| `CommentReadMode.Fail`           | Throw a `SectionNotFoundException` when the section does not exist, or throw a `PropertyNotFoundException` when the section exists but the property does not exist. |
| `CommentReadMode.Fallback`       | Fall back to none comment if the section or property does not exist. |
| `CommentReadMode.CustomFallback` | Fall back to a custom fallback comment if the section or property does not exist. |

[&#8593;](#features)
---

### Writing the comment of a section
```cs
const string ini = """
                   [Section]
                   Property=Value
                   """;

const string comment = """
                       This is a sample
                       multiline
                       comment. :)
                       """;

using var textReader = new StringReader(ini);
var iniDocument = IniDocument.LoadFrom(textReader, IniDocumentConfiguration.Default);

iniDocument.WriteComment("Section", comment);

using var textWriter = new StringWriter();
iniDocument.SaveTo(textWriter);

textWriter.Flush();

Console.WriteLine(textWriter);
```

Writing the comment of a section supports the following modes:

| Mode                      | Description |
|---------------------------|-------------|
| `CommentWriteMode.Fail`   | Throw a `SectionNotFoundException` when the section does not exist. |
| `CommentWriteMode.Ignore` | Silently ignore if the section does not exist. |

[&#8593;](#features)
---

### Writing the comment of a property
```cs
const string ini = """
                   [Section]
                   Property=Value
                   """;

const string comment = """
                       This is a sample
                       multiline
                       comment. :)
                       """;

using var textReader = new StringReader(ini);
var iniDocument = IniDocument.LoadFrom(textReader, IniDocumentConfiguration.Default);

iniDocument.WriteComment("Section", "Property", comment);

using var textWriter = new StringWriter();
iniDocument.SaveTo(textWriter);

textWriter.Flush();

Console.WriteLine(textWriter);
```

Writing the comment of a property supports the following modes:

| Mode                      | Description |
|---------------------------|-------------|
| `CommentWriteMode.Fail`   | Throw a `SectionNotFoundException` when the section does not exist, or throw a `PropertyNotFoundException` when the section exists but the property does not exist. |
| `CommentWriteMode.Ignore` | Silently ignore if the section or the property does not exist. |

[&#8593;](#features)
---

## License
MIT License

[https://opensource.org/license/mit](https://opensource.org/license/mit)

## Socialize
If you like or use my work and you are interested in this kind of software development let's get in touch. :)
