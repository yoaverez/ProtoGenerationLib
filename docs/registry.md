# The Registry object

As his name suggests, the registry object is an object that manages the registration of the library [Customizations](customization.md).

Each `ProtoGenerator` object contains a unique registry object.
In most cases, you only need to generate protos once. If this is not the case for you, you can reuse the same `ProtoGenerator` over and over.
If for some reason, you can not use the same custom converters or custom mappers (see [User Defined Custom Converters](customization.md#user-defined-custom-converters) and [User Defined Custom Type Mappers](customization.md#user-defined-custom-type-mappers)), you can do this by creating a new `ProtoGenerator` object that will have its own independent registry object.

## Registry Methods

### Custom Converters Registry Methods

| Method Name | Description |
|-------------|-------------|
| `RegisterDataTypeCustomConverter` | Register the given converter to the registry's data types custom converters collection. |
| `RegisterContractTypeCustomConverter` | Register the given converter to the registry's contracts type custom converters collection. |
| `RegisterEnumTypeCustomConverter` | Register the given converter to the registry's enums type custom converters collection. |

### Custom Type Mappers Registry Methods

| Method Name | Description |
|-------------|-------------|
| `RegisterCustomTypeNameMapper` | Register the given mapper to the registry's custom type mappers collection. |

### Custom Field Suffixes Registry Methods

| Method Declaration | Description |
|-------------|-------------|
| `RegisterCustomFieldSuffix<TFieldType>(string suffix)` | Register that a field with the given `TFieldType`, should have a suffix addition to their name. The suffix will be appended only if there will be no more specific suffix addition constraint. |
| `RegisterCustomFieldSuffix<TFieldDeclaringType>(string fieldName, string suffix)` | Register that a field with the given `fieldName` that is declared in the `TFieldDeclaringType`, should have a suffix addition to their name. The suffix will be appended only if there will be no more specific suffix addition constraint. |
| `RegisterCustomFieldSuffix<TFieldDeclaringType, TFieldType>(string suffix)` | Register that a field with the given `TFieldType` that was declared in the given `TFieldDeclaringType`, should have a suffix addition to their name. The suffix will be appended only if there will be no more specific suffix addition constraint. |
| `RegisterFieldThatShouldNotHaveSuffix<TFieldDeclaringType, TFieldType>(string fieldName)` | Register that a field with the given `TFieldType` that was declared in the given `TFieldDeclaringType` and have the given `fieldName`, should **not** have a suffix addition to their name. |

### Strategies Registry Methods

#### Styling Strategies Registry Methods

| Method Name | Description |
|-------------|-------------|
| `RegisterProtoStylingStrategy` | Register the given styling strategy with it's associated name to the registry. |
| `RegisterPackageStylingStrategy` | Register the given package styling strategy with it's associated name to the registry. |
| `RegisterFilePathStylingStrategy` | Register the given file path styling strategy with it's associated name to the registry. |

#### Naming Strategies Registry Methods

| Method Name | Description |
|-------------|-------------|
| `RegisterTypeNamingStrategy` | Register the given type naming strategy with it's associated name to the registry. |
| `RegisterPackageNamingStrategy` | Register the given package naming strategy with it's associated name to the registry. |
| `RegisterFileNamingStrategy` | Register the given file naming strategy with it's associated name to the registry. |
| `RegisterParameterListNamingStrategy` | Register the given parameter list naming strategy with it's associated name to the registry. |
| `RegisterNewTypeNamingStrategy` | Register the given new type naming strategy with it's associated name to the registry. |

#### Extraction Strategies Registry Methods

| Method Name | Description |
|-------------|-------------|
| `RegisterFieldsAndPropertiesExtractionStrategy` | Register the given fields and properties extraction strategy with it's associated name to the registry. |

#### Numbering Strategies Registry Methods

| Method Name | Description |
|-------------|-------------|
| `RegisterFieldNumberingStrategy` | Register the given field numbering strategy with it's associated name to the registry. |
| `RegisterEnumValueNumberingStrategy` | Register the given enum value numbering strategy with it's associated name to the registry. |