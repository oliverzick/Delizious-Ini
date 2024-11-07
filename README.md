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

Upcoming features:
* Configurability of case sensitivity
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

## License
MIT License

[https://opensource.org/license/mit](https://opensource.org/license/mit)

## Socialize
If you like or use my work and you are interested in this kind of software development let's get in touch. :)
