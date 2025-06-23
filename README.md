# ProtoGenerator

## Table of Contents

1. [Introduction](#1-introduction)
2. [Installation](#2-installation)
3. [Getting Started](#3-getting-started)

## 1. Introduction

The C# Proto Generator is a flexible and extensible library designed to generate Protocol Buffer (proto) files from C# classes without requiring attribute decoration. It emphasis flexibility, allowing for customization at multiple levels.

### 1.1 General Usage of Communication DTOs Generation

Communication DTOs generation is especially valuable when aiming to decouple the communication contract from the internal service implementations. By generating communication DTOs directly from your C# models, you can:

- Avoid coupling transport details (like gRPC, HTTP, or messaging frameworks) to your core logic
- Allow multiple services or clients to consume the same message definitions over different communication protocols
- Enable shared contracts across microservices or layers, independent of their internal implementation
- Maintain clean separation of concerns and support for layered or hexagonal architecture styles

It is recommended to use the general proto generation when:
- You want to share data contracts across services without enforcing implementation patterns
- You plan to use multiple communication methods (e.g., REST, gRPC, message queues) on the same service contracts
- You wish to generate proto definitions as part of a CI/CD pipeline without impacting source code

### 1.2 Library Purpose

While there are existing libraries that generate proto files from C# classes, they typically require decorating classes with attributes. This library aims to provide a non-intrusive solution that can generate proto files from existing C# classes without modifying them while allowing external customizations.

### 1.3 Key Features

- Can generate proto files from C# classes with or without attribute decoration
- Support for complex type hierarchies and relationships
- Flexible type mapping system for converting C# types to proto types
- Customizable naming strategies for packages, messages, fields, and enums
- Handling of nested types and collections

## 2. Installation

### 📦 NuGet Package (recommended)

Install the ProtoGenerator via NuGet Package Manager:

```sh
dotnet add package ProtoGenerator
```

Or using the Package Manager Console in Visual Studio:

```powershell
Install-Package ProtoGenerator
```

### 🛠️ Build from Source

If you want to build the library from source:

1. Clone the repository:

   ```sh
   git clone https://github.com/your-org/ProtoGenerator.git
   cd ProtoGenerator
   ```

2. Build the project:

   ```sh
   dotnet build
   ```

3. (Optional) Reference the built DLL in your project.

## 3. Getting Started

The simplest and shortest way to generate proto files from C# classes:

```csharp
// Create an instance of the ProtoGenerator main class.
var protoGenerator = new ProtoGenerator();

// The most recommended types to give are the types that define the services
// since from them, the generator will be able to generate the rest of the types.
var csharpServicesType = new Type[] 
{ 
    /* 
    Add here the types that define the services 
    or any other types that you want to convert to protos.
    */ 
}

// Call the GenerateProtos method.
var protoDefinitions = protoGenerator.GenerateProtos(csharpServicesType)

// Chose what to do with the definitions.
// You can write them to files.
protoDefinitions.WriteToFiles("<The path to the proto root>", "<The path from the proto root in which to write all the protos>");

// Or you can write them to strings.
var protos = protoDefinitions.WriteToFiles();
```

This example utilize the default configurations objects.

You can also change the configuration or add specific customizations.

To change the configuration of the generator, just edit the default configuration object `ProtoGenerationOptions.Default` or create new default configuration object by calling the `ProtoGenerationOptions.CreateDefaultProtoGenerationOptions` method and pass it to the `GenerateProtos` method.

For more information, see:
* [Configuration Guide](docs/configuration.md)
* [Customization Guide](docs/customization.md)
* [Pre Defined Strategies Guide](docs/pre-defined-strategies.md)
* [Registry Object](docs/registry.md)