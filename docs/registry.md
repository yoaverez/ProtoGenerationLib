# The Registry object

As his name suggests, the registry object is an object that manages the registration of the library's strategies.

Each `ProtoGenerator` object contains a unique registry object.
In most cases, you only need to generate protos once. If this is not the case for you, you can reuse the same `ProtoGenerator` over and over.

## Registry Methods

### Styling Strategies Registry Methods

| Method Name | Description |
|-------------|-------------|
| `RegisterProtoStylingStrategy` | Register the given styling strategy with it's associated name to the registry. |
| `RegisterPackageStylingStrategy` | Register the given package styling strategy with it's associated name to the registry. |
| `RegisterFilePathStylingStrategy` | Register the given file path styling strategy with it's associated name to the registry. |

### Naming Strategies Registry Methods

| Method Name | Description |
|-------------|-------------|
| `RegisterTypeNamingStrategy` | Register the given type naming strategy with it's associated name to the registry. |
| `RegisterPackageNamingStrategy` | Register the given package naming strategy with it's associated name to the registry. |
| `RegisterFileNamingStrategy` | Register the given file naming strategy with it's associated name to the registry. |
| `RegisterParameterListNamingStrategy` | Register the given parameter list naming strategy with it's associated name to the registry. |
| `RegisterNewTypeNamingStrategy` | Register the given new type naming strategy with it's associated name to the registry. |

### Extraction Strategies Registry Methods

| Method Name | Description |
|-------------|-------------|
| `RegisterFieldsAndPropertiesExtractionStrategy` | Register the given fields and properties extraction strategy with it's associated name to the registry. |
| `RegisterDocumentationExtractionStrategy` | Register the given documentation extraction strategy with it's associated name to the registry. |
| `RegisterMethodSignatureExtractionStrategy` | Register the given method signature extraction strategy with it's associated name to the registry. |

### Numbering Strategies Registry Methods

| Method Name | Description |
|-------------|-------------|
| `RegisterFieldNumberingStrategy` | Register the given field numbering strategy with it's associated name to the registry. |
| `RegisterEnumValueNumberingStrategy` | Register the given enum value numbering strategy with it's associated name to the registry. |