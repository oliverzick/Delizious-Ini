# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.20.0] - 2025-07-16
### Documented
- Document available property enumeration modes in README ([#186](https://github.com/oliverzick/Delizious-Ini/issues/186))
- Document available section deletion modes in README ([#188](https://github.com/oliverzick/Delizious-Ini/issues/188))
- Document available property deletion modes in README ([#190](https://github.com/oliverzick/Delizious-Ini/issues/190))
- Document available comment read modes for reading the comment of a section in README ([#192](https://github.com/oliverzick/Delizious-Ini/issues/192))
- Document available comment read modes for reading the comment of a property in README ([#194](https://github.com/oliverzick/Delizious-Ini/issues/194))
- Document available comment write modes for writing the comment of a section in README ([#197](https://github.com/oliverzick/Delizious-Ini/issues/197))
- Document available comment write modes for writing the comment of a property in README ([#199](https://github.com/oliverzick/Delizious-Ini/issues/199))
- Document available property read modes in README ([#202](https://github.com/oliverzick/Delizious-Ini/issues/202))
- Document available property write modes in README ([#204](https://github.com/oliverzick/Delizious-Ini/issues/204))

## [1.19.0] - 2025-06-02
### Documented
- Streamline XML doc for thrown exceptions ([#183](https://github.com/oliverzick/Delizious-Ini/issues/183))
- Link features and chapters in README ([#184](https://github.com/oliverzick/Delizious-Ini/issues/184))

## [1.18.0] - 2025-05-04
### Added
- Enable configuration of the comment string that indicates the beginning of a comment line in an INI document ([#175](https://github.com/oliverzick/Delizious-Ini/issues/175))

## [1.17.0] - 2025-04-14
### Added
- Enable writing the comment for a section of an INI document with given section name and default comment write mode ([#170](https://github.com/oliverzick/Delizious-Ini/issues/170))
- Enable writing the comment for a property of an INI document with given section name, property key and default comment write mode ([#173](https://github.com/oliverzick/Delizious-Ini/issues/173))

## [1.16.0] - 2025-04-06
### Added
- Enable writing the comment for a property of an INI document with given section name, property key and comment write mode ([#168](https://github.com/oliverzick/Delizious-Ini/issues/168))

## [1.15.0] - 2025-03-30
### Added
- Enable writing the comment for a section of an INI document with given section name and comment write mode ([#160](https://github.com/oliverzick/Delizious-Ini/issues/160))

## [1.14.0] - 2025-03-15
### Added
- Enable reading the comment of a section from INI document for given section name and default comment read mode ([#150](https://github.com/oliverzick/Delizious-Ini/issues/150))
- Enable reading the comment of a property from INI document for given section name, property key and default comment read mode ([#158](https://github.com/oliverzick/Delizious-Ini/issues/158))

## [1.13.0] - 2025-03-09
### Added
- Enable reading the comment of a property from INI document for given section name, property key and mode ([#146](https://github.com/oliverzick/Delizious-Ini/issues/146))
  - Fail mode: Throw section not found exception when section does not exist, throw property not found exception when property does not exist
  - Fallback mode: Return fallback comment given by the mode when the section or property does not exist

## [1.12.0] - 2025-03-06
### Added
- Enable reading the comment of a section from INI document for given section name and mode ([#142](https://github.com/oliverzick/Delizious-Ini/issues/142))
  - Fail mode: Throw section not found exception when section does not exist
  - Fallback mode: Return fallback comment given by the mode when the section does not exist

## [1.11.0] - 2025-02-22
### Added
- Enable cloning an INI document ([#140](https://github.com/oliverzick/Delizious-Ini/issues/140))

## [1.10.0] - 2025-02-17
### Added
- Enable configuration of the regular expression pattern for a section name in an INI document ([#138](https://github.com/oliverzick/Delizious-Ini/issues/138))

## [1.9.0] - 2025-02-05
### Added
- Enable configuration of the newline string used in an INI document ([#135](https://github.com/oliverzick/Delizious-Ini/issues/135))

## [1.8.0] - 2025-01-23
### Added
- Enable configuration of the beginning delimiter of a section in an INI document ([#131](https://github.com/oliverzick/Delizious-Ini/issues/131))
- Enable configuration of the end delimiter of a section in an INI document ([#133](https://github.com/oliverzick/Delizious-Ini/issues/133))

## [1.7.0] - 2025-01-19
### Added
- Enable configuration of duplicate property behavior that specifies how an INI document should behave on loading when a duplicate property occurs ([#128](https://github.com/oliverzick/Delizious-Ini/issues/128))
  * Fail: Throws persistence exception when a duplicate property occurs
  * Ignore: Ignores subsequent occurrences of a duplicate property by using the first occurrence of such a property
  * Override: Overrides previous occurrences of a duplicate property by using the last occurrence of such a property

## [1.6.0] - 2025-01-11
### Added
- Enable configuration of duplicate section behavior that specifies how an INI document should behave on loading when a duplicate section occurs ([#124](https://github.com/oliverzick/Delizious-Ini/issues/124))
  * Fail: Throws persistence exception when a duplicate section occurs
  * Merge: Merges a duplicate section

## [1.5.0] - 2025-01-04
### Added
- Enable configuration of the assignment spacer of a property in an INI document ([#122](https://github.com/oliverzick/Delizious-Ini/issues/122))

## [1.4.0] - 2025-01-01
### Added
- Enable configuration of the assignment separator of a property in an INI document ([#120](https://github.com/oliverzick/Delizious-Ini/issues/120))

## [1.3.0] - 2024-12-29
### Added
- Enable configuration of invalid line behavior that specifies how an INI document should behave on loading when a line is invalid and cannot be parsed ([#117](https://github.com/oliverzick/Delizious-Ini/issues/117))

## [1.2.0] - 2024-12-27
### Added
- Provide dedicated loose INI document configuration ([#111](https://github.com/oliverzick/Delizious-Ini/issues/111))
- Provide dedicated strict INI document configuration ([#112](https://github.com/oliverzick/Delizious-Ini/issues/112))

## [1.1.0] - 2024-12-23
### Added
- Enable providing record-like string representation of INI document configuration ([#109](https://github.com/oliverzick/Delizious-Ini/issues/109))

## [1.0.0] - 2024-12-22
### Added
- Enable value semantics for INI document configuration ([#102](https://github.com/oliverzick/Delizious-Ini/issues/102))

## [0.27.2] - 2024-12-15
### Fixed
- Fix resetting case sensitivity to default when specifying another setting in INI document configuration ([#105](https://github.com/oliverzick/Delizious-Ini/issues/105))

## [0.27.1] - 2024-12-15
### Fixed
- Fix missing argument check in INI document configuration ([#103](https://github.com/oliverzick/Delizious-Ini/issues/103))

## [0.27.0] - 2024-12-14
### Added
- Enable configuration of case sensitivity that specifies how to treat section names and property keys in an INI document ([#100](https://github.com/oliverzick/Delizious-Ini/issues/100))

## [0.26.0] - 2024-08-28
### Added
- Specify loose modes for default INI document configuration ([#90](https://github.com/oliverzick/Delizious-Ini/issues/90))

    |Mode                     |Default |
    |-------------------------|--------|
    |Property enumeration mode|Fallback|
    |Property read mode       |Fallback|
    |Property write mode      |Create  |
    |Property deletion mode   |Ignore  |
    |Section deletion mode    |Ignore  |

## [0.25.0] - 2024-08-25
### Added
- Enable deletion of a property from an INI document with a configured default property deletion mode ([#86](https://github.com/oliverzick/Delizious-Ini/issues/86))

## [0.24.0] - 2024-08-24
### Added
- Enable deletion of a section from an INI document with a configured default section deletion mode ([#84](https://github.com/oliverzick/Delizious-Ini/issues/84))

## [0.23.0] - 2024-08-18
### Added
- Enable configuration of the default property write mode for an INI document ([#81](https://github.com/oliverzick/Delizious-Ini/issues/81))

## [0.22.0] - 2024-08-14
### Added
- Enable configuration of the default property read mode for an INI document ([#79](https://github.com/oliverzick/Delizious-Ini/issues/79))

## [0.21.0] - 2024-08-13
### Added
- Enable configuration of the default property enumeration mode for an INI document ([#77](https://github.com/oliverzick/Delizious-Ini/issues/77))

## [0.20.0] - 2024-08-07
### Added
- Introduce INI document configuration ([#75](https://github.com/oliverzick/Delizious-Ini/issues/75))

## [0.19.0] - 2024-08-05
### Added
- Enable creating a new empty INI document ([#73](https://github.com/oliverzick/Delizious-Ini/issues/73))

## [0.18.0] - 2024-07-05
### Added
- Enable deletion of a property in an INI document with a given mode ([#69](https://github.com/oliverzick/Delizious-Ini/issues/69))
  - Fail mode: Throw section not found exception if the section does not exist, throw property not found exception if the section exists but the property to delete does not exist
  - Ignore mode: Silently ignore if the section or the property to delete does not exist

## [0.17.0] - 2024-06-30
### Changed
- Improve performance by superseding exception-based approach by strategy-based approach ([#70](https://github.com/oliverzick/Delizious-Ini/issues/70))

## [0.16.0] - 2024-06-05
### Added
- Enable deletion of a section in an INI document with a given mode ([#67](https://github.com/oliverzick/Delizious-Ini/issues/67))
  - Fail mode: Throw section not found exception if the section to delete does not exist
  - Ignore mode: Silently ignore if the section to delete does not exist

## [0.15.0] - 2024-05-23
### Added
- Introduce property write mode to create a new property with the given value ([#65](https://github.com/oliverzick/Delizious-Ini/issues/65))

## [0.14.0] - 2024-05-21
### Changed
- Expose factory properties instead of factory methods for predefined modes ([#61](https://github.com/oliverzick/Delizious-Ini/issues/61))

## [0.13.0] - 2024-05-20
### Added
- Enable writing the value of a property contained in a section of an INI document with a given mode that specifies the write behavior ([#59](https://github.com/oliverzick/Delizious-Ini/issues/59))
  - Update mode: Throw section not found exception when section does not exist, throw property not found exception when property does not exist

## [0.12.0] - 2024-05-05
### Added
- Enable specification of mode when reading a property contained in a section ([#55](https://github.com/oliverzick/Delizious-Ini/issues/55))
  - Fail mode: Throw section not found exception when section does not exist, throw property not found exception when property does not exist
  - Fallback mode: Return fallback property value given by the mode when the section or property does not exist

## [0.11.0] - 2024-05-04
### Changed
- Improve name of method to update property ([#52](https://github.com/oliverzick/Delizious-Ini/issues/52))

## [0.10.0] - 2024-05-04
### Changed
- Improve name of method to read property ([#42](https://github.com/oliverzick/Delizious-Ini/issues/42))

## [0.9.0] - 2024-05-01
### Added
- Enable specification of mode when enumerating properties contained in a section of an INI document ([#39](https://github.com/oliverzick/Delizious-Ini/issues/39))
  - Fail mode: Throw section not found exception when section does not exist
  - Fallback mode: Enumerate empty collection of properties when section does not exist

## [0.8.0] - 2024-04-29
### Changed
- Improve name of method to enumerate properties ([#37](https://github.com/oliverzick/Delizious-Ini/issues/37))

## [0.7.0] - 2024-04-29
### Changed
- Improve name of method to enumerate sections ([#35](https://github.com/oliverzick/Delizious-Ini/issues/35))

## [0.6.0] - 2024-04-21
### Changed
- Streamline persistence methods to load and save an INI document ([#18](https://github.com/oliverzick/Delizious-Ini/issues/18))
  - Improve naming of persistence methods by streamline naming to `LoadFrom` and `SaveTo`
  - Throw more specific persistence exception when loading or saving an INI document failed

## [0.5.0] - 2024-04-20
### Added
- Enable serialization of an INI document to a text writer ([#16](https://github.com/oliverzick/Delizious-Ini/issues/16))

## [0.4.2] - 2024-02-12
### Fixed
- Fix possible specification of property key that must not be empty or consist only of white-space characters ([#14](https://github.com/oliverzick/Delizious-Ini/issues/14))

## [0.4.1] - 2024-02-12
### Fixed
- Fix possible specification of section name that must not be empty or consist only of white-space characters ([#12](https://github.com/oliverzick/Delizious-Ini/issues/12))

## [0.4.0] - 2024-02-12
### Added
- Enable updating the value of a property, specified by the name of the containing section and its property key, in an INI document ([#9](https://github.com/oliverzick/Delizious-Ini/issues/9))

## [0.3.0] - 2024-02-11
### Added
- Enable reading the value of a property, specified by the name of the containing section and its property key, from INI document ([#3](https://github.com/oliverzick/Delizious-Ini/issues/3))

## [0.2.0] - 2024-02-11
### Added
- Enable retrieving all property keys for a section, specified by its section name, from INI document ([#4](https://github.com/oliverzick/Delizious-Ini/issues/4))

## [0.1.0] - 2024-02-06
### Added
- Enable retrieving all section names from INI document ([#1](https://github.com/oliverzick/Delizious-Ini/issues/1))