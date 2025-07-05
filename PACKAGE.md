## About

ProtoGenerationLib is a flexible and extensible library designed to generate Protocol Buffer (proto) files from C# classes without requiring attribute decoration. It emphasis flexibility, allowing for customization at multiple levels.

## Key Features

- Can generate proto files from C# classes with or without attribute decoration
- Support for complex type hierarchies and relationships
- Flexible type mapping system for converting C# types to proto types
- Customizable naming strategies for packages, messages, fields, and enums
- Handling of nested types and collections
- Support custom field names suffixes
- Support custom documentation of the generated protos

## How to Use
```cs
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
var protos = protoDefinitions.WriteToStrings();
```

## Additional Documentation

For more information go to the [Project Repository](https://github.com/yoaverez/ProtoGenerationLib)