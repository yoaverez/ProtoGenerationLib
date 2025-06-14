using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Constants;
using ProtoGenerator.Converters.Abstracts;
using ProtoGenerator.Converters.Internals.CSharpToProtoDefinition;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.Models.Internals.ProtoDefinitions;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Utilities.CollectionUtilities;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Converters.Internals
{
    /// <summary>
    /// Converter from csharp types to proto types representations.
    /// </summary>
    public class CSharpToProtoConverter : ICSharpToProtoTypesConverter
    {
        /// <summary>
        /// Converter from csharp contract type to proto service definition.
        /// </summary>
        private ICSharpToProtoTypeConverter<IServiceDefinition> contractToServiceConverter;

        /// <summary>
        /// Converter from csharp data type to proto message definition.
        /// </summary>
        private ICSharpToProtoTypeConverter<IMessageDefinition> dataTypeToMessageConverter;

        /// <summary>
        /// Converter from csharp enum type to proto enum definition.
        /// </summary>
        private ICSharpToProtoTypeConverter<IEnumDefinition> enumToEnumDefinitionConverter;

        /// <summary>
        /// The well known types which doesn't need conversion.
        /// </summary>
        private ISet<Type> wellKnownTypes;

        /// <summary>
        /// Create new instance of the <see cref="CSharpToProtoConverter"/> class.
        /// </summary>
        /// <param name="componentsProvider">A provider of all the proto generator customizations.</param>
        /// <param name="contractToServiceConverter"><inheritdoc cref="contractToServiceConverter" path="/node()"/></param>
        /// <param name="dataTypeToMessageConverter"><inheritdoc cref="dataTypeToMessageConverter" path="/node()"/></param>
        /// <param name="enumToEnumDefinitionConverter"><inheritdoc cref="enumToEnumDefinitionConverter" path="/node()"/></param>
        /// <param name="enumToEnumDefinitionConverter"><inheritdoc cref="enumToEnumDefinitionConverter" path="/node()"/></param>
        public CSharpToProtoConverter(IProvider componentsProvider,
                                      ICSharpToProtoTypeConverter<IServiceDefinition>? contractToServiceConverter = null,
                                      ICSharpToProtoTypeConverter<IMessageDefinition>? dataTypeToMessageConverter = null,
                                      ICSharpToProtoTypeConverter<IEnumDefinition>? enumToEnumDefinitionConverter = null,
                                      ISet<Type>? wellKnownTypes = null)
        {
            this.contractToServiceConverter = contractToServiceConverter ?? new ContractTypeToServiceConverter(componentsProvider);
            this.dataTypeToMessageConverter = dataTypeToMessageConverter ?? new DataTypeToMessageConverter(componentsProvider);
            this.enumToEnumDefinitionConverter = enumToEnumDefinitionConverter ?? new EnumTypeToEnumDefinitionConverter(componentsProvider);
            this.wellKnownTypes = wellKnownTypes ?? WellKnownTypesConstants.WellKnownTypes.Keys.ToHashSet();
        }

        /// <inheritdoc/>
        public IDictionary<string, IProtoDefinition> Convert(IEnumerable<Type> types,
                                                             IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                             IProtoGenerationOptions generationOptions)
        {
            var fileToFileDefinition = new Dictionary<string, IProtoDefinition>();
            var serviceAttribute = generationOptions.AnalysisOptions.ProtoServiceAttribute;
            foreach (var type in types.ToHashSet())
            {
                // Can not and should not convert well known types.
                if (wellKnownTypes.Contains(type))
                    continue;

                // Proto nested types will be dealt in their
                // declaring type.
                if (protoTypesMetadatas[type].IsNested)
                    continue;

                var fileDefinition = GetOrAddProtoDefinition(fileToFileDefinition, type, protoTypesMetadatas[type], generationOptions.ProtoFileSyntax);

                IProtoObject protoObject;
                if (type.IsEnum)
                {
                    var enumDefinition = enumToEnumDefinitionConverter.ConvertTypeToProtoDefinition(type, protoTypesMetadatas, generationOptions);
                    fileDefinition.Enums.Add(enumDefinition);
                    protoObject = enumDefinition;
                }
                else if(type.IsDefined(serviceAttribute, serviceAttribute.IsAttributeInherited()))
                {
                    var serviceDefinition = contractToServiceConverter.ConvertTypeToProtoDefinition(type, protoTypesMetadatas, generationOptions);
                    fileDefinition.Services.Add(serviceDefinition);
                    protoObject = serviceDefinition;
                }
                else
                {
                    var messageDefinition = dataTypeToMessageConverter.ConvertTypeToProtoDefinition(type, protoTypesMetadatas, generationOptions);
                    fileDefinition.Messages.Add(messageDefinition);
                    protoObject = messageDefinition;
                }

                fileDefinition.Imports.AddRange(protoObject.Imports);
            }

            return fileToFileDefinition;
        }

        /// <summary>
        /// Get the <see cref="ProtoDefinition"/> in which the given
        /// <paramref name="type"/> should be declared.
        /// If the <see cref="ProtoDefinition"/> does not exist, creates a
        /// new one.
        /// </summary>
        /// <param name="fileToFileDefinition">The mapping between file paths to their <see cref="IProtoDefinition"/>.</param>
        /// <param name="type">The type whose declaring <see cref="ProtoDefinition"/> is requested.</param>
        /// <param name="metadata">The metadata of the given <paramref name="type"/>.</param>
        /// <param name="protoFileSyntax">The proto file syntax (The line in the head of a proto file).</param>
        /// <returns>
        /// The <see cref="ProtoDefinition"/> in which the given
        /// <paramref name="type"/> should be declared.
        /// </returns>
        /// <exception cref="Exception">
        /// Thrown when a <see cref="ProtoDefinition"/> exists for the given
        /// <paramref name="type"/> but the given <paramref name="type"/> package
        /// is different from the package of the <see cref="ProtoDefinition"/>.
        /// </exception>
        private static ProtoDefinition GetOrAddProtoDefinition(IDictionary<string, IProtoDefinition> fileToFileDefinition,
                                                               Type type,
                                                               IProtoTypeMetadata metadata,
                                                               string protoFileSyntax)
        {
            var fileRelativePath = metadata.FilePath!;
            var typePackage = metadata.Package!;

            ProtoDefinition fileDefinition;
            if (fileToFileDefinition.TryGetValue(fileRelativePath, out var existingFileDefinition))
            {
                fileDefinition = (existingFileDefinition as ProtoDefinition)!;
                if (!fileDefinition.Package.Equals(typePackage))
                    throw new Exception($"The file with the relative path of {fileRelativePath}" +
                        $"contains at least two different packages:{Environment.NewLine}" +
                        $"1: {fileDefinition.Package}{Environment.NewLine}" +
                        $"2: {typePackage}{Environment.NewLine}" +
                        $"The second package originated from the type: {type.Name}.");
            }
            else
            {
                fileDefinition = new ProtoDefinition
                {
                    Package = typePackage,
                    Syntax = protoFileSyntax,
                };
                fileToFileDefinition.Add(fileRelativePath, fileDefinition);
            }

            return fileDefinition;
        }
    }
}
