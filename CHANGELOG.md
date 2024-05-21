# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.14.0] - 2024-05-21
### Added
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