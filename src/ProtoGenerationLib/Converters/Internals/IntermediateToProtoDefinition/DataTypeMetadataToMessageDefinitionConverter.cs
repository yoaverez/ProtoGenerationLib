using ProtoGenerationLib.Models.Internals.ProtoDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using static ProtoGenerationLib.Converters.Internals.IntermediateToProtoDefinition.IntermediateToProtoDefinitionUtils;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Utilities.TypeUtilities;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Utilities.CollectionUtilities;

namespace ProtoGenerationLib.Converters.Internals.IntermediateToProtoDefinition
{
    /// <summary>
    /// A converter between data type intermediate representation to its proto message representation.
    /// </summary>
    public class DataTypeMetadataToMessageDefinitionConverter : IIntermediateToProtoDefinitionConverter<IDataTypeMetadata, IMessageDefinition>
    {
        /// <summary>
        /// A provider of all the proto generator customizations.
        /// </summary>
        private IProvider componentsProvider;

        /// <summary>
        /// A converter that converts enum type metadata representation
        /// to an enum proto definition representation.
        /// </summary>
        private IIntermediateToProtoDefinitionConverter<IEnumTypeMetadata, IEnumDefinition> enumTypeMetadataToEnumDefinitionConverter;

        /// <summary>
        /// Create new instance of the <see cref="DataTypeMetadataToMessageDefinitionConverter"/> class.
        /// </summary>
        /// <param name="componentsProvider"><inheritdoc cref="componentsProvider" path="/node()"/></param>
        /// <param name="enumTypeMetadataToEnumDefinitionConverter"><inheritdoc cref="enumTypeMetadataToEnumDefinitionConverter" path="/node()"/></param>
        public DataTypeMetadataToMessageDefinitionConverter(IProvider componentsProvider,
                                                            IIntermediateToProtoDefinitionConverter<IEnumTypeMetadata, IEnumDefinition>? enumTypeMetadataToEnumDefinitionConverter = null)
        {
            this.componentsProvider = componentsProvider;
            this.enumTypeMetadataToEnumDefinitionConverter = enumTypeMetadataToEnumDefinitionConverter ?? new EnumTypeMetadataToEnumDefinitionConverter(componentsProvider);
        }

        /// <inheritdoc/>
        public IMessageDefinition ConvertIntermediateRepresentationToProtoDefinition(IDataTypeMetadata intermediateType,
                                                                                     IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                                                     IProtoGenerationOptions generationOptions)
        {
            var imports = new HashSet<string>();
            var protoTypeMetadata = protoTypesMetadatas[intermediateType.Type];
            var fields = new List<IFieldDefinition>();
            var numOfFields = intermediateType.Fields.Count();
            for (int fieldIdx = 0; fieldIdx < numOfFields; fieldIdx++)
            {
                fields.Add(CreateFieldDefinitionFromFieldMetadata(intermediateType.Fields.ElementAt(fieldIdx), fieldIdx, numOfFields, protoTypeMetadata.FullName!, generationOptions, protoTypesMetadatas, out var neededImports));
                imports.AddRange(neededImports);
            }

            var nestedMessages = new List<IMessageDefinition>();
            foreach (var nestedTypeMetadata in intermediateType.NestedDataTypes)
            {
                if (protoTypeMetadata.NestedTypes.Contains(nestedTypeMetadata.Type))
                {
                    var nestedMessage = ConvertIntermediateRepresentationToProtoDefinition(nestedTypeMetadata,
                                                                                           protoTypesMetadatas,
                                                                                           generationOptions);
                    nestedMessages.Add(nestedMessage);
                    imports.AddRange(nestedMessage.Imports);
                }
            }

            var nestedEnums = new List<IEnumDefinition>();
            foreach (var nestedTypeMetadata in intermediateType.NestedEnumTypes)
            {
                if (protoTypeMetadata.NestedTypes.Contains(nestedTypeMetadata.Type))
                {
                    nestedEnums.Add(enumTypeMetadataToEnumDefinitionConverter.ConvertIntermediateRepresentationToProtoDefinition(nestedTypeMetadata,
                                                                                                                                 protoTypesMetadatas,
                                                                                                                                 generationOptions));
                }
            }

            return new MessageDefinition(protoTypeMetadata.Name!,
                                         protoTypeMetadata.Package!,
                                         imports,
                                         fields,
                                         nestedMessages,
                                         nestedEnums);
        }

        /// <summary>
        /// Create a <see cref="IFieldDefinition"/> from the given <paramref name="fieldMetadata"/>.
        /// </summary>
        /// <param name="fieldMetadata">The field metadata from which to create the <see cref="IFieldDefinition"/>.</param>
        /// <param name="fieldIndex">The index of the field.</param>
        /// <param name="numOfFields">The total number of fields.</param>
        /// <param name="messageFullName">The full name of the message in which this field will be declared.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <param name="protoTypesMetadatas">A mapping between type to its proto type metadata.</param>
        /// <param name="neededImports">The imports that are needed in the file in order to use the field type.</param>
        /// <returns>
        /// A <see cref="IFieldDefinition"/> that represents the given <paramref name="fieldMetadata"/>.
        /// </returns>
        private IFieldDefinition CreateFieldDefinitionFromFieldMetadata(IFieldMetadata fieldMetadata,
                                                                        int fieldIndex,
                                                                        int numOfFields,
                                                                        string messageFullName,
                                                                        IProtoGenerationOptions generationOptions,
                                                                        IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                                        out ISet<string> neededImports)
        {
            neededImports = new HashSet<string>();
            var numberingStrategy = componentsProvider.GetFieldNumberingStrategy(generationOptions.NumberingStrategiesOptions.FieldNumberingStrategy);
            var fieldNumber = numberingStrategy.GetFieldNumber(fieldMetadata, fieldIndex, numOfFields);

            var fieldName = componentsProvider.GetProtoStylingStrategy(generationOptions.ProtoStylingConventionsStrategiesOptions.FieldStylingStrategy).ToProtoStyle(fieldMetadata.Name);

            var packageComponentsSeparator = componentsProvider.GetPackageStylingStrategy(generationOptions.ProtoStylingConventionsStrategiesOptions.PackageStylingStrategy).PackageComponentsSeparator;
            string typeName;
            var fieldRule = FieldRule.None;
            if (fieldMetadata.Type.TryGetElementsOfKeyValuePairEnumerableType(out var keyType, out var valueType))
            {
                var keyTypeShortName = GetTypeShortName(protoTypesMetadatas[keyType].FullName, messageFullName, packageComponentsSeparator);
                var valueTypeShortName = GetTypeShortName(protoTypesMetadatas[valueType].FullName, messageFullName, packageComponentsSeparator);
                typeName = $"map<{keyTypeShortName}, {valueTypeShortName}>";

                neededImports.Add(protoTypesMetadatas[keyType].FilePath!);
                neededImports.Add(protoTypesMetadatas[valueType].FilePath!);
            }
            else if (!fieldMetadata.Type.IsMultiDimensionalOrJaggedArray() && fieldMetadata.Type.TryGetElementOfEnumerableType(out var elementType))
            {
                fieldRule = FieldRule.Repeated;
                typeName = GetTypeShortName(protoTypesMetadatas[elementType].FullName, messageFullName, packageComponentsSeparator);

                neededImports.Add(protoTypesMetadatas[elementType].FilePath!);
            }
            else if (fieldMetadata.Type.TryGetElementOfNullableType(out var nullableElementType))
            {
                fieldRule = FieldRule.Optional;
                typeName = GetTypeShortName(protoTypesMetadatas[nullableElementType].FullName, messageFullName, packageComponentsSeparator);

                neededImports.Add(protoTypesMetadatas[nullableElementType].FilePath!);
            }
            else
            {
                if (IsOptionalField(fieldMetadata, generationOptions.AnalysisOptions.OptionalFieldAttribute))
                {
                    fieldRule = FieldRule.Optional;
                }

                typeName = GetTypeShortName(protoTypesMetadatas[fieldMetadata.Type].FullName, messageFullName, packageComponentsSeparator);

                neededImports.Add(protoTypesMetadatas[fieldMetadata.Type].FilePath!);
            }
            return new FieldDefinition(fieldName, typeName, fieldNumber, fieldRule);
        }

        /// <summary>
        /// Checks whether or not the given <paramref name="fieldMetadata"/> is
        /// an optional field.
        /// </summary>
        /// <param name="fieldMetadata">The field metadata.</param>
        /// <param name="optionalFieldAttribute">The type of the optional field attribute.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="fieldMetadata"/> should
        /// be an optional field in the proto message otherwise <see langword="false"/>.
        /// </returns>
        public bool IsOptionalField(IFieldMetadata fieldMetadata, Type optionalFieldAttribute)
        {
            return fieldMetadata.Attributes.Any(attribute => attribute.GetType().Equals(optionalFieldAttribute));
        }
    }
}
