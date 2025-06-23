# Pre Defined Strategies Guide

## Pre Defined Strategies Names Lookup

In order to make your life easier when trying to use any of the pre-defined strategies, this library provides lookup tables that converts enum values to strategies names.

Each pre-defined strategy is associated with a single enum value.

The lookup tables are contained within the `StrategyNamesLookup` static object.

The following example shows how to use the lookup tables for changing the enum value numbering strategy:

```csharp
ProtoGenerationOptions.Default.EnumValueNumberingStrategy = StrategyNamesLookup.EnumValueNumberingStrategiesLookup[EnumValueNumberingStrategyKind.SameAsEnumValue];
```

## Styling Strategies

| Strategy Type | Associated Enum Value | Description |
|---------------|-----------------------|-------------|
| `CamelCaseStrategy` | `ProtoStylingStrategyKind.`<br>`CamelCase` | Format string as a camelCase string. e.g. Hello World becomes helloWorld. |
| `UpperCamelCaseStrategy` | `ProtoStylingStrategyKind.`<br>`UpperCamelCase` | Format string as a UpperCamelCase string. e.g. Hello World becomes HelloWorld. |
| `SnakeCaseStrategy` | `ProtoStylingStrategyKind.`<br>`SnakeCase` | Format string as a snake_case string. e.g. Hello World becomes hello_world. |
| `UpperSnakeCaseStrategy` | `ProtoStylingStrategyKind.`<br>`UpperSnakeCase` | Format string as a UPPER_SNAKE_CASE string. e.g. Hello World becomes HELLO_WORLD. |
| `DotDelimitedSnakeCaseStrategy` | `ProtoStylingStrategyKind.`<br>`DotDelimitedSnakeCase` | Format string as a dot delimited snake case string. e.g if the words are: ILikeApple, MeToo then the result will be i_like_apple.me_too |
| `ForwardSlashDelimitedSnakeCaseStrategy` | `FilePathStylingStrategyKind.`<br>`ForwardSlashDelimitedSnakeCase` | Format string as a forward slash delimited snake case string. e.g if the words are: ILikeApple, MeToo then the result will be i_like_apple/me_too. If the words represent a proto file path (as it should) like: ComputerUsers, File.proto then the result will be computer_users/file.proto |

## Naming Strategies

### File Naming Strategies

| Strategy Type | Associated Enum Value | Description |
|---------------|-----------------------|-------------|
| `SingleFileStrategy` | `FilePathStrategyKind.`<br>`SingleFileNamedProtos` | A file name strategy that puts all types in a single proto file. The pre defined strategy is defined with the file name of "protos.proto". You can use this strategy if you want a single file with different name. |
| `TypeNameAsFileNameStrategy` | `FilePathStrategyKind.`<br>`TypeName` | A file naming strategy in which the file name will be the type name. |
| `NameSpaceAndTypeNameAsFileNameStrategy` | `FilePathStrategyKind.`<br>`NameSpaceAndTypeName` | A file naming strategy that groups types by their namespace and their names. This strategy allows you to structure the protos in the same path structures as your C# DTOs (if your DTOs are separated to type per file and the files are grouped in directories). |
| `NameSpaceAsFileNameStrategy` | `FilePathStrategyKind.`<br>`NameSpace` | A file name strategy that groups types by their namespace. This is a risky strategy that if your DTOs are not structured correctly, may cause a recursive import to happen inside the proto files. |

### Package Naming Strategies

| Strategy Type | Associated Enum Value | Description |
|---------------|-----------------------|-------------|
| `ConstNameAsPackageStrategy` | `PackageNamingStrategyKind.`<br>`SinglePackageNamedProtos` |  A package naming strategy where all types share the same package. The pre defined strategy is defined with the package name of "protos". You can use this strategy if you want a single package with different name. |
| `NameSpaceAsPackageStrategy` | `PackageNamingStrategyKind.`<br>`NameSpaceAsPackageName` |  A package naming strategy where the package of a type is its namespace. |

### Type Naming Strategies

| Strategy Type | Associated Enum Value | Description |
|---------------|-----------------------|-------------|
| `TypeNameAsAlphaNumericTypeNameStrategy` | `TypeNamingStrategyKind.`<br>`TypeNameAsAlphaNumericTypeName` |  A type naming strategy where the proto name is the same as the type name. |

### New Type Naming Strategies

| Strategy Type | Associated Enum Value | Description |
|---------------|-----------------------|-------------|
| `NewTypeNamingStrategy` | `NewTypeNamingStrategyKind.`<br>`NameAsAlphaNumericTypeName` |  Naming strategy for new types that uses the name of the type if the type is not generic otherwise uses the name without generics and a combination of the generic arguments names. |
| `ParameterListNamingStrategy` | `ParameterListNamingStrategyKind.`<br>`MethodNameAndParametersTypes` |  A parameter list naming strategy that names a parameter list based on the method name with a suffix. |

## Numbering Strategies

| Strategy Type | Associated Enum Value | Description |
|---------------|-----------------------|-------------|
| `EnumNumberSameAsEnumValueStrategy` | `EnumValueNumberingStrategyKind.`<br>`SameAsEnumValue` |  Enum value numbering strategy that assigns the same value of the enum value as the enum value. |
| `SequentialEnumValueNumberingStrategy` | `EnumValueNumberingStrategyKind.`<br>`Sequential` |  Enum value numbering strategy that assigns the index of the enum value as the enum value. |
| `SequentialFieldNumberingStrategy` | `FieldNumberingStrategyKind.`<br>`Sequential` |  Field numbering strategy that assigns the index of the field as the field number. |

## Analysis Strategies

| Strategy Type | Associated Enum Value | Description |
|---------------|-----------------------|-------------|
| `CompositeFieldsAndPropertiesExtractionStrategy` | `FieldsAndPropertiesExtractionStrategyKind.`<br>`Composite` |  Field and properties extraction strategy that composite base type to a single field. |
| `FlattenedFieldsAndPropertiesExtractionStrategy` | `FieldsAndPropertiesExtractionStrategyKind.`<br>`Flatten` |  Field and properties extraction strategy that flattened all the fields and property of the type. i.e. each field or property of base class or implemented interface will be taken as a single member. |