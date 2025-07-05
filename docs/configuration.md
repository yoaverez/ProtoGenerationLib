# Configuration Guide
 
## The Generation Options

The library configuration is contained inside the `ProtoGenerationOptions` object.

This class contains properties for following configuration groups:

### 1. Styling Conventions Options

The styling options allow you to style your proto in almost any way you want by picking styling strategies to use.

You can style the following things:

* Messages names
* Services names
* Enums names
* Enum values names
* Fields names
* Packages
* Rpcs names
* File paths

The following properties are available:

| Property Name      | Default Value | Description |
|--------------------|---------------|-------------|
| `MessageStylingStrategy` | "UpperCamelCase" | The name of the message name styling strategy. |
| `EnumStylingStrategy` | "UpperCamelCase" | The name of the enum name styling strategy. |
| `EnumValueStylingStrategy` | "UpperSnakeCase" | The name of the enum value name styling strategy. |
| `ServiceStylingStrategy` | "UpperCamelCase" | The name of the service name styling strategy. |
| `FieldStylingStrategy` | "SnakeCase" | The name of the field name styling strategy. |
| `PackageStylingStrategy` | "DotDelimitedSnakeCase" | The name of the package name styling strategy. |
| `RpcStylingStrategy` | "UpperCamelCase" | The name of the rpc name styling strategy. |
| `FilePathStylingStrategy` | "ForwardSlashDelimitedSnakeCase" | The name of the file path styling strategy. |

For a list of all the pre defined strategies, check the [Styling Strategies Section](pre-defined-strategies.md#styling-strategies) in the [Pre Defined Strategies Guide](pre-defined-strategies.md).

If you don't want to use any of pre-defined strategies, you can create your on strategies. For more information, check the [User Defined Strategies Section](customization.md#user-defined-strategies) in the [Customization Guide](customization.md).

### 2. Proto Type Naming Options

The type naming options allow you to pick the proto types names for your C# types in a global manner.
i.e. in a way that will effect all the types except for types that were excluded using specific customizations.

You can name the following things:

* Types
* Packages
* File paths

**Note**: The styling strategies from the [Styling Conventions Options](#1-styling-conventions-options), are applied after the naming strategies except for types that were excluded using specific customizations.
For more information, check the [User Defined Custom Type Mappers Section](customization.md#user-defined-custom-type-mappers) in the [Customization Guide](customization.md).

The following properties are available:

| Property Name      | Default Value | Description |
|--------------------|---------------|-------------|
| `TypeNamingStrategy` | "TypeNameAsAlphaNumericTypeName" | The name of the strategy that responsible for giving a C# type its proto type name. |
| `PackageNamingStrategy` | "SinglePackageNamedProtos" | The name of the strategy that responsible for getting a C# type its proto package name. |
| `FileNamingStrategy` | "SingleFileNamedProtos" | The name of the strategy that responsible for giving a C# type its proto file path. |

For a list of all the pre defined strategies, check the [Naming Strategies Section](pre-defined-strategies.md#naming-strategies) in the [Pre Defined Strategies Guide](pre-defined-strategies.md).

If you don't want to use any of pre-defined strategies, you can create your on strategies. For more information, check the [User Defined Strategies Section](customization.md#user-defined-strategies) in the [Customization Guide](customization.md).

### 3. New Type Naming Options

There are things that C# allows but do not exist in Protocol Buffers (Protobuf). 

For example, Protobuf does not support rpcs with multiple request parameters.

Another example is fields with complex enumerable types like `IEnumerable<int[]>`. In Protobuf,
you can not apply any rule (in this case the `repeated` rule) more than ones.

Therefore, in order to generate complex C# data types or services types, this library can create new types to represent the complex types.

Those new types need names.

The new type naming options allows you to control over new types:

* New type representing rpc parameter list

* New type representing a regular C# type that does not exists in the Protobuf language.

The following properties are available:

| Property Name      | Default Value | Description |
|--------------------|---------------|-------------|
| `ParameterListNamingStrategy` | "MethodNameAndParametersTypes" | The name of the strategy that names a new type that represents a method parameter list. |
| `NewTypeNamingStrategy` | "NewTypeNamingAsAlphaNumericTypeName" | The name of the strategy that names a new type that is needed in order to generate the protos. |

For a list of all the pre defined strategies, check the [New Type Naming Strategies Section](pre-defined-strategies.md#new-type-naming-strategies) in the [Pre Defined Strategies Guide](pre-defined-strategies.md).

If you don't want to use any of pre-defined strategies, you can create your on strategies. For more information, check the [User Defined Strategies Section](customization.md#user-defined-strategies) in the [Customization Guide](customization.md).

### 4. Numbering Options

In Protocol Buffers (Protobuf), each field in a message must have a unique positive number. In addition, Enum values also must have numbers and must have the zero value first.

The numbering options allow you more control over those numbers.

As mentioned, you can control the numbering of the following things:

* Messages fields
* Enum values

The following properties are available:

| Property Name      | Default Value | Description |
|--------------------|---------------|-------------|
| `FieldNumberingStrategy` | "Sequential" | The name of the strategy that responsible for matching a proto field number to a csharp field. |
| `EnumValueNumberingStrategy` | "Sequential" | The name of the strategy that responsible for matching a proto enum value number to a csharp enum value. |

For a list of all the pre defined strategies, check the [Numbering Strategies Section](pre-defined-strategies.md#numbering-strategies) in the [Pre Defined Strategies Guide](pre-defined-strategies.md).

If you don't want to use any of pre-defined strategies, you can create your on strategies. For more information, check the [User Defined Strategies Section](customization.md#user-defined-strategies) in the [Customization Guide](customization.md).

### 5. Analysis Options

The analysis options allows you to configure the analysis of your C# types.

The analysis options composed from the following properties:

#### Analysis Options Flags

| Property Name      | Default Value | Description |
|--------------------|---------------|-------------|
| `IncludeFields` | false | Whether or not to include csharp fields in the proto message fields. |
| `IncludePrivates` | false | Whether or not to include private properties (or fields if `IncludeFields` is true). |
| `IncludeStatics` | false | Whether or not to include static properties (and static fields if `IncludeFields` is true). |
| `RemoveEmptyMembers` | true | Whether or not to remove properties (or fields if `IncludeFields` is true) of types that are considered empty. |

**Notes**: 

1. Public properties are always analyzed.

2. It is not recommended to use the `IncludePrivates` flags. Communication data types should have only public members. If you have very little and specific types with privates fields/properties that you want to include in your protos, you should use the custom converter customization. For more information, check the [User Defined Custom Converters Section](customization.md#user-defined-custom-converters) in the[Customization Guide](customization.md).

#### Analysis Options Strategies

| Property Name      | Default Value | Description |
|--------------------|---------------|-------------|
| `FieldsAndPropertiesExtractionStrategy` | "Composite" | The name of the strategy for extracting properties (and fields if `IncludeFields` is true) from csharp types. |
| `DocumentationExtractionStrategy` | "None" | The name of the strategy for extracting documentation from csharp entities. |

For a list of all the pre defined strategies, check the [Extraction Strategies Section](pre-defined-strategies.md#extraction-strategies) in the [Pre Defined Strategies Guide](pre-defined-strategies.md).

If you don't want to use any of pre-defined strategies, you can create your on strategies. For more information, check the [User Defined Strategies Section](customization.md#user-defined-strategies) in the [Customization Guide](customization.md).

#### Analysis Options Attributes

| Property Name      | Default Value | Description |
|--------------------|---------------|-------------|
| `IgnoreFieldOrPropertyAttribute` | `typeof(ProtoIgnoreAttribute)` | The type of the attribute that if used means that the analysis should ignore the specific field or property. |
| `DataTypeConstructorAttribute` | `typeof(ProtoMessageConstructorAttribute)` | The type of the constructor attribute that tells that the constructor contains all the wanted fields and properties of the type. |
| `ProtoServiceAttribute` | `typeof(ProtoServiceAttribute)` | The type of the attribute that tells that the class/interface/struct represents a proto service. |
| `ProtoRpcAttribute` | `typeof(ProtoRpcAttribute)` | The type of the attribute that tells that the method represents a proto rpc method. |
| `OptionalFieldAttribute` | `typeof(OptionalDataMemberAttribute)` | The type of the attribute that tells that the field/property represents an optional proto field. |

**Note:** This library does not need the above attributes to work. They exist for
the user convenience. They can be replace by the [Analysis Options Delegates](#analysis-options-delegates) or by [User Defined Custom Converters](customization.md#user-defined-custom-converters).

#### Analysis Options Delegates

| Property Name      | Default Value | Description |
|--------------------|---------------|-------------|
| `IsProtoServiceDelegate` | `(type) => false` | A delegate for checking if a type is a proto service. This delegate comes in addition to the `ProtoServiceAttribute` |
| `TryGetRpcTypeDelegate` | `(Type serviceType, MethodInfo method, out ProtoRpcType rpcType) => { rpcType = ProtoRpcType.Unary; return false; }` | A delegate for trying to get the `ProtoRpcType` from a service method. This delegate comes in addition to the `ProtoRpcAttribute` |

#### Analysis Options Providers

| Property Name      | Description |
|--------------------|-------------|
| `DocumentationProvider` | A provider for user documentation customization. |

### 6. Customization Properties

| Property Name      | Default Value | Description |
|--------------------|---------------|-------------|
| `ContractTypeCustomConverters`  | Empty collection | A collection containing all the contract type custom converters. The first converter that can handle a type will be the only converter that will handle the type. |
| `DataTypeCustomConverters`  | Empty collection | A collection containing all the data type custom converters. The first converter that can handle a type will be the only converter that will handle the type. |
| `EnumTypeCustomConverters`  | Empty collection | A collection containing all the enum type custom converters. The first converter that can handle a type will be the only converter that will handle the type. |
| `CustomTypeMappers`  | Empty collection | A collection containing all the custom type mappers. The first mapper that can handle a type will be the only mapper that will handle the type. |

### 7. Other Properties

| Property Name      | Default Value | Description |
|--------------------|---------------|-------------|
| `ProtoFileSyntax`  | "proto3" | The proto file syntax (The line in the head of a proto file). Should be either "proto2" or "proto3". |

### 8. Methods

#### Customization Adder Methods

##### Field Suffixes Adder Methods

| Method signature   | Description |
|--------------------|-------------|
| `void AddFieldSuffix<TFieldType>(string suffix)` | Add the given suffix to the field suffixes collection. This suffix will be appended to all the fields names of type TFieldType. |
| `void AddFieldSuffix<TFieldDeclaringType>(string fieldName, string suffix)` | Add the given suffix to the field suffixes collection. This suffix will be appended to the field with the given fieldName in the TFieldDeclaringType. |
| `void AddFieldSuffix<TFieldDeclaringType, TFieldType>(string suffix)` | Add the given suffix to the field suffixes collection. This suffix will be appended to all the fields names of type TFieldType that are declared in the given TFieldDeclaringType. |
| `void AddFieldThatShouldNotHaveSuffix<TFieldDeclaringType, TFieldType>(string fieldName)` | Add the given fieldName to a collection excluded fields. i.e fields of type TFieldType that was declared in the given TFieldDeclaringType and should not get a suffix appended to them. |

##### Custom Documentation Adder Methods

| Method signature   | Description |
|--------------------|-------------|
| `void AddTypeDocumentation<TType>(string documentation)` | Associate the given documentation with the given TType. |
| `void AddFieldDocumentation<TFieldDeclaringType>(string fieldName, string documentation)` | Associate the given documentation with the field with the given fieldName that is declared in the given TFieldDeclaringType. |
| `void AddMethodDocumentation<TMethodDeclaringType>(string methodName, int numOfParameters, string documentation)` | Associate the given documentation with the method with the given methodName that is declared in the given TMethodDeclaringType. |
| `void AddEnumValueDocumentation<TEnumType>(int enumValue, string documentation)` | Associate the given documentation with the given enumValue. |

#### Customization Getters Methods

| Method signature   | Description |
|--------------------|-------------|
| `IEnumerable<ICustomTypesExtractor> GetCustomTypesExtractors()` | Get all the user defined types extractors. |
| `IEnumerable<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>> GetContractTypeCustomConverters()` | Get all the user defined contract types converters. |
| `IEnumerable<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>> GetDataTypeCustomConverters()` | Get all the user defined data types converters. |
| `IEnumerable<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>> GetEnumTypeCustomConverters()` | Get all the user defined enum types converters. |
| `IEnumerable<ICustomTypeMapper> GetCustomTypeMappers()` | Get all the user defined type mappers. |
| `bool TryGetFieldSuffix(Type fieldDeclaringType, Type fieldType, string fieldName, out string suffix)` | Try getting the suffix of the field with the given fieldName of fieldType that was declared in the given fieldDeclaringType. |

## The Serialization Options

The serialization options contains configurations for serializing the resulted proto definitions to strings or files.

The following properties are available:

| Property Name      | Default Value | Description |
|--------------------|---------------|-------------|
| `IndentSize` | 2 | The indentation size in spaces. e.g. IndentSize = x means that each tab/indentation is x spaces. |