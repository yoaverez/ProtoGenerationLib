# Customization Guide

This library provide different layers of customization for customizing your proto generation.

## Configuration Customization

The configurations dictates which strategies to use, which attributes to use,
how to analyze C# DTOs and more.

For more information check the [Configuration Guide](configuration.md).

## User Defined Strategies

In addition to the library [Pre-Defined Strategies](pre-defined-strategies.md), it enables the usage of external defined strategies.

### Adding New Strategy Step by Step

The following example illustrate how to register new `Field Numbering Strategy`.

1. Pick the kind of the strategy that you want to add - in this case it is the field numbering strategy.
2. Check which interface do you need to implement - in this case it is the `IFieldNumberingStrategy`.
3. Create a new class that implements the above interface.
4. Pick a unique name for that strategy.
5. Register the new strategy with the chosen name to the [Registry Object](registry.md) using the [Strategies Registry Methods](registry.md#strategies-registry-methods).
6. Set the configuration to use the new strategy (see [Configuration Guide](configuration.md)).

## User Defined Custom Converters

If you have few C# types that needs special treatment for the way that they will be converted to Protobuf structure (Service Message or Enum), you should create custom converters for those types.

A custom converter controls over the conversion between one or more C# types to their intermediate representation. A type intermediate representation, contains all the information that is needed in order to convert the C# type to a proto type. For example, a custom converter for data type should contains data such as the fields of the type, the nested data types and the nested enum types.

In addition to the conversion, custom converters should also implement an extraction method for the extraction of used type. The library need this information to know that it may need to generate protos for those types.

Fortunately, this library has some abstract classes for you that abstract the implementation of the used types extraction.

* `CSharpContractTypeToContractTypeMetadataCustomConverter` - Abstract custom converter for converting C# contract types to their meta data (intermediate representation).
* `CSharpDataTypeToDataTypeMetadataCustomConverter`- Abstract custom converter for converting C# data types to their meta data (intermediate representation).
* `CSharpEnumTypeToEnumTypeMetadataCustomConverter` - Abstract custom converter for converting C# enum types to their meta data (intermediate representation).

In each of the abstract classes there are two abstract methods:

1. ```csharp
     bool CanHandle(Type type, IProtoGenerationOptions generationOptions);
    ```
    
2. ```csharp
     TIntermediate BaseConvertTypeToIntermediateRepresentation(Type type,  ProtoGenerationOptions generationOptions);
    ```
    Where the `TIntermediate` will be replace with the correct intermediate type.

### Adding New Custom Converter Step by Step

1. Pick a type (or a group of types that behave similarly).
2. Decide if they are contract types (Services), data types (Messages) or enum types.
3. Create a new class that inherits from the abstract converter that matches to the types proto structure (contract, data or enum).
4. Implement the above abstract methods.
5. Register the custom converter to the [Registry Object](registry.md) using the [Custom Converters Registry Methods](registry.md#custom-converters-registry-methods).

**Note:** The first custom converter that can handle a type, will handle the type. Meaning that if you have more than one custom converter that can handle the same type, you should register the more specific converter first.

## User Defined Custom Type Mappers

Custom type mapper allows you to exclude specific types naming and styling from the naming and styling strategies that were chosen in the configuration. With a custom type mapper, you can set a C# type proto type name, package, file path and whether this type should be created (in case you add a mapper for well known type or for a type that you already have a proto type for, there is no need in creating new proto type).
You can choose to set only some of the above and the rest will be determined by the naming and styling strategies mentioned above.

A custom type mapper has two methods:
1. ```csharp
     bool CanHandle(Type type);
    ```
    
2. ```csharp
     IProtoTypeBaseMetadata MapTypeToProtoMetadata(Type type);
    ```

### Adding New Custom Type Mapper Step by Step

1. Pick a type (or a group of types that behave similarly).
2. Create a new class that implements the `ITypeMapper` interface.
3. Register the custom type mapper to the [Registry Object](registry.md) using the [Custom Type Mappers Registry Methods](registry.md#custom-type-mappers-registry-methods).

**Note:** The first custom type mapper that can handle a type, will handle the type. Meaning that if you have more than one custom type mapper that can handle the same type, you should register the more specific mapper first.

## User Defines Custom Field Suffixes

Custom field suffixes allow you to describe fields better. A common use of this is for adding unit suffixes to fields names.

For example you can make all `TimeSpan` fields to have the `UTC` suffix or make
`Velocity` fields have different units like `MetersPerSec`, `KMPerSec`, `SpeedOfLight`, `Knots` and more.

You can add custom field suffixes by using the registry's [Custom Field Suffixes Registry Methods](registry.md#custom-field-suffixes-registry-methods).