using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Discovery.Abstracts;
using ProtoGenerator.Mappers.Abstracts;
using ProtoGenerator.Mappers.Internals;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.Models.Internals.ProtoDefinitions;
using ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Utilities.CollectionUtilities;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerator.Discovery.Internals
{
    /// <inheritdoc cref="IProtoTypeMetadataDiscoverer"/>
    public class ProtoTypeMetadataDiscoverer : IProtoTypeMetadataDiscoverer
    {
        /// <summary>
        /// Provider for all the proto generation customizations.
        /// </summary>
        private IProvider componentsProvider;

        /// <summary>
        /// The default type mappers.
        /// </summary>
        private IEnumerable<ITypeNameMapper> defaultTypeMappers;

        /// <summary>
        /// The separator that should be used by the user and by the library in order
        /// to separate package components.
        /// </summary>
        private const string PACKAGE_COMPONENTS_SEPARATOR = ".";

        /// <summary>
        /// Create new instance of the <see cref="ProtoTypeMetadataDiscoverer"/> class.
        /// </summary>
        /// <param name="componentsProvider"><inheritdoc cref="componentsProvider" path="/node()"/></param>
        /// <param name="defaultTypeMappers"><inheritdoc cref="defaultTypeMappers" path="/node()"/></param>
        public ProtoTypeMetadataDiscoverer(IProvider componentsProvider, IEnumerable<ITypeNameMapper>? defaultTypeMappers = null)
        {
            this.componentsProvider = componentsProvider;
            this.defaultTypeMappers = defaultTypeMappers ?? DefaultTypeMappersCreator.CreateDefaultTypeMappers();
        }

        /// <inheritdoc/>
        public IReadOnlyDictionary<Type, IProtoTypeMetadata> DiscoverProtosMetadata(IEnumerable<Type> types,
                                                                                    IProtoGenerationOptions protoGeneratorConfiguration)
        {
            var userDefinedTypeMappers = componentsProvider.GetCustomTypeNameMappers();

            // Note that the default mappers comes before the user mappers
            // since the default mappers are meant for types like primitives
            // and well known types whose name, package and file path are set and
            // can not be changed.
            var typeMappers = defaultTypeMappers.Concat(userDefinedTypeMappers).ToArray();

            var typesSet = types.ToHashSet();
            var metadatas = new Dictionary<Type, IProtoTypeMetadata>();
            var styledBaseMetadatas = new Dictionary<Type, IProtoTypeBaseMetadata>();
            var typeToDescendants = new Dictionary<Type, ISet<Type>>();
            foreach (var type in typesSet)
            {
                IProtoTypeBaseMetadata styledBaseMetadata;
                if (TryGetProtoTypeMetadataFromMappers(type, typeMappers, out var mapperBaseMetadata))
                {
                    styledBaseMetadata = FillMapperBaseMetadata(mapperBaseMetadata, type, protoGeneratorConfiguration);
                }
                else
                {
                    var name = GetProtoTypeName(type, protoGeneratorConfiguration);
                    var package = GetPackageName(type, protoGeneratorConfiguration);
                    var filePath = GetFilePath(type, protoGeneratorConfiguration);
                    var unstyledBaseMetadata = new ProtoTypeBaseMetadata(name, package, filePath);
                    styledBaseMetadata = StyleBaseMetadata(type, unstyledBaseMetadata, protoGeneratorConfiguration);
                }

                styledBaseMetadatas.Add(type, styledBaseMetadata);
            }

            // Compute the full name of the type.
            foreach (var typeAndStyledBaseMetadata in styledBaseMetadatas)
            {
                var type = typeAndStyledBaseMetadata.Key;
                var styledBaseMetadata = typeAndStyledBaseMetadata.Value;
                var fullName = ComputeFullName(type, styledBaseMetadatas, out var declaringType);
                var metadata = new ProtoTypeMetadata(styledBaseMetadata, fullName);

                if (declaringType is not null)
                {
                    metadata.IsNested = true;
                    if (!typeToDescendants.ContainsKey(declaringType))
                    {
                        typeToDescendants.Add(declaringType, new HashSet<Type>());
                    }
                    typeToDescendants[declaringType].Add(type);
                }

                metadatas.Add(type, metadata);
            }

            // Fill the descendants.
            foreach (var typeAndItsMetadata in metadatas)
            {
                var type = typeAndItsMetadata.Key;
                var metadata = typeAndItsMetadata.Value;
                if (typeToDescendants.ContainsKey(type))
                {
                    metadata.NestedTypes.AddRange(typeToDescendants[type]);
                }
            }

            return metadatas;
        }

        #region Name getting Auxiliaries

        /// <summary>
        /// Gets the proto name of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose proto name you want.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>The proto name of the given <paramref name="type"/>.</returns>
        private string GetProtoTypeName(Type type,
                                        IProtoGenerationOptions generationOptions)
        {
            var namingStrategy = componentsProvider.GetTypeNamingStrategy(generationOptions.ProtoNamingStrategiesOptions.TypeNamingStrategy);
            return namingStrategy.GetTypeName(type);
        }

        /// <summary>
        /// Get the styled package name of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose package name to retrieve.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>The styled package name of the given <paramref name="type"/>.</returns>
        private string GetPackageName(Type type,
                                      IProtoGenerationOptions generationOptions)
        {
            var packageNamingStrategy = componentsProvider.GetPackageNamingStrategy(generationOptions.ProtoNamingStrategiesOptions.PackageNamingStrategy);
            var unstyledPackageComponents = packageNamingStrategy.GetPackageComponents(type);
            var unstyledPackage = string.Join(PACKAGE_COMPONENTS_SEPARATOR, unstyledPackageComponents);
            return unstyledPackage;
        }

        /// <summary>
        /// Gets the file path of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose file path is requested.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>The file path of the given <paramref name="type"/>.</returns>
        private string GetFilePath(Type type, IProtoGenerationOptions generationOptions)
        {
            var fileNamingStrategy = componentsProvider.GetFileNamingStrategy(generationOptions.ProtoNamingStrategiesOptions.FileNamingStrategy);
            return fileNamingStrategy.GetFilePath(type);
        }

        /// <summary>
        /// Compute the full proto name of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose full name is requested.</param>
        /// <param name="protosBaseMetadatas">
        /// The base metadatas of all the csharp types that are needed
        /// for the proto generation.
        /// </param>
        /// <param name="declaringType">The declaring type of the given <paramref name="type"/> if exists otherwise null.</param>
        /// <returns>The full proto name of the given <paramref name="type"/>.</returns>
        private string ComputeFullName(Type type,
                                       IReadOnlyDictionary<Type, IProtoTypeBaseMetadata> protosBaseMetadatas,
                                       out Type? declaringType)
        {
            declaringType = null;

            var typeBaseMetadata = protosBaseMetadatas[type];
            var package = typeBaseMetadata.Package;
            var declaringTypesPlusName = typeBaseMetadata.Name!;

            // If the type is a proto primitive then
            // it full name is just his name.
            if (package!.Equals(string.Empty))
            {
                return declaringTypesPlusName;
            }

            var currType = type;
            while (currType.TryGetDeclaringType(out var currDeclaringType) && protosBaseMetadatas.ContainsKey(currDeclaringType))
            {
                var declaringTypeMetadata = protosBaseMetadatas[currDeclaringType];
                if (!typeBaseMetadata.FilePath!.Equals(declaringTypeMetadata.FilePath)
                   || !typeBaseMetadata.Package!.Equals(declaringTypeMetadata.Package))
                {
                    // This type won't be a descendant of the declaring type in the
                    // proto files.
                    break;
                }

                // Else the given type will definitely be a descendant of
                // the declaring type in the proto files.
                declaringTypesPlusName = $"{declaringTypeMetadata.Name}.{declaringTypesPlusName}";
                currType = currDeclaringType;
                if (declaringType is null)
                    declaringType = currDeclaringType;
            }

            return $"{package}.{declaringTypesPlusName}";
        }

        #endregion Name getting Auxiliaries

        #region Name Styling Auxiliaries

        /// <summary>
        /// Create a new <see cref="IProtoTypeBaseMetadata"/> from the given <paramref name="unstyledBaseMetadata"/>
        /// after applying styles to the properties.
        /// </summary>
        /// <param name="type">The type from which the given <paramref name="unstyledBaseMetadata"/> was created.</param>
        /// <param name="unstyledBaseMetadata">The unstyled proto type base metadata.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>
        /// A new <see cref="IProtoTypeBaseMetadata"/> from the given <paramref name="unstyledBaseMetadata"/>
        /// after applying styles to the properties.
        /// </returns>
        private IProtoTypeBaseMetadata StyleBaseMetadata(Type type, IProtoTypeBaseMetadata unstyledBaseMetadata, IProtoGenerationOptions generationOptions)
        {
            var styledName = StyleProtoName(type, unstyledBaseMetadata.Name!, generationOptions);

            var styledPackage = StyleProtoPackage(type, unstyledBaseMetadata.Package!, generationOptions);

            return new ProtoTypeBaseMetadata(styledName, styledPackage, unstyledBaseMetadata.FilePath!);
        }

        /// <summary>
        /// Get the style proto type name of the given <paramref name="unstyledName"/>.
        /// </summary>
        /// <param name="type">The type whose proto unstyled name is the given <paramref name="unstyledName"/>.</param>
        /// <param name="unstyledName">The unstyled proto name of the given <paramref name="type"/>.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>The style proto type name of the given <paramref name="unstyledName"/>.</returns>
        private string StyleProtoName(Type type, string unstyledName, IProtoGenerationOptions generationOptions)
        {
            var protoServiceAttribute = generationOptions.AnalysisOptions.ProtoServiceAttribute;
            IProtoStylingStrategy stylingStrategy;
            if (type.IsEnum)
            {
                stylingStrategy = componentsProvider.GetProtoStylingStrategy(generationOptions.ProtoStylingConventionsStrategiesOptions.EnumStylingStrategy);
            }
            else if (type.IsDefined(protoServiceAttribute, protoServiceAttribute.IsAttributeInherited()))
            {
                stylingStrategy = componentsProvider.GetProtoStylingStrategy(generationOptions.ProtoStylingConventionsStrategiesOptions.ServiceStylingStrategy);
            }
            else
            {
                stylingStrategy = componentsProvider.GetProtoStylingStrategy(generationOptions.ProtoStylingConventionsStrategiesOptions.MessageStylingStrategy);
            }

            var styledName = stylingStrategy.ToProtoStyle(unstyledName);
            return styledName;
        }

        /// <summary>
        /// Get the styled proto type package of the given <paramref name="unstyledPackage"/>.
        /// </summary>
        /// <param name="type">The type whose proto unstyled package is the given <paramref name="unstyledPackage"/>.</param>
        /// <param name="unstyledPackage">The unstyled proto package of the given <paramref name="type"/>.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>The style proto type package of the given <paramref name="unstyledPackage"/>.</returns>
        private string StyleProtoPackage(Type type, string unstyledPackage, IProtoGenerationOptions generationOptions)
        {
            var packageComponents = unstyledPackage.Split(new string[] { PACKAGE_COMPONENTS_SEPARATOR }, StringSplitOptions.RemoveEmptyEntries);
            return StyleProtoPackage(type, packageComponents, generationOptions);
        }

        /// <summary>
        /// Get the styled proto type package of the given <paramref name="unstyledPackage"/>.
        /// </summary>
        /// <param name="type">The type whose proto unstyled package is the given <paramref name="unstyledPackage"/>.</param>
        /// <param name="packageComponents">The unstyled proto package components of the given <paramref name="type"/>.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>The style proto type package of the given <paramref name="unstyledPackage"/>.</returns>
        private string StyleProtoPackage(Type type, string[] packageComponents, IProtoGenerationOptions generationOptions)
        {
            var packageStylingStrategy = componentsProvider.GetPackageStylingStrategy(generationOptions.ProtoStylingConventionsStrategiesOptions.PackageStylingStrategy);
            var styledPackage = packageStylingStrategy.ToProtoStyle(packageComponents);
            return styledPackage;
        }

        #endregion Name Styling Auxiliaries

        #region Auxiliary Functions

        /// <summary>
        /// Fill all the null properties of the given <paramref name="mapperBaseMetadata"/>.
        /// </summary>
        /// <param name="mapperBaseMetadata">The base metadata from the mapper.</param>
        /// <param name="type">The type that originate the base metadata.</param>
        /// <param name="protoGeneratorConfiguration">The generation options.</param>
        /// <returns>A new <see cref="IProtoTypeBaseMetadata"/> that represents the filled version of the given <paramref name="mapperBaseMetadata"/>.</returns>
        private IProtoTypeBaseMetadata FillMapperBaseMetadata(IProtoTypeBaseMetadata mapperBaseMetadata,
                                                              Type type,
                                                              IProtoGenerationOptions protoGeneratorConfiguration)
        {
            var styledName = mapperBaseMetadata.Name;
            var styledPackage = mapperBaseMetadata.Package;

            if (styledName is null)
            {
                var unstyledName = GetProtoTypeName(type, protoGeneratorConfiguration);
                styledName = StyleProtoName(type, unstyledName, protoGeneratorConfiguration);
            }

            if (styledPackage is null)
            {
                var unstyledPackage = GetPackageName(type, protoGeneratorConfiguration);
                styledPackage = StyleProtoPackage(type, unstyledPackage, protoGeneratorConfiguration);
            }
            var filePath = mapperBaseMetadata.FilePath ?? GetFilePath(type, protoGeneratorConfiguration);
            var styledBaseMetadata = new ProtoTypeBaseMetadata(styledName, styledPackage, filePath);
            return styledBaseMetadata;
        }

        /// <summary>
        /// Try getting the given <paramref name="type"/>s proto type metadata from the given <paramref name="mappers"/>.
        /// </summary>
        /// <param name="type">The csharp type whose proto type metadata is requested.</param>
        /// <param name="mappers">The mappers.</param>
        /// <param name="protoTypeMetadata">
        /// If there is a mapper that can map this <paramref name="type"/> then the mapper
        /// return metadata otherwise <see cref="string.Empty"/>.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/> can be mapped by
        /// any of the given <paramref name="mappers"/> otherwise <see langword="false"/>.
        /// </returns>
        private static bool TryGetProtoTypeMetadataFromMappers(Type type, IEnumerable<ITypeNameMapper> mappers, out IProtoTypeBaseMetadata protoTypeMetadata)
        {
            protoTypeMetadata = default;
            foreach (var mapper in mappers)
            {
                if (mapper.CanHandle(type))
                {
                    protoTypeMetadata = mapper.MapTypeToProtoMetaData(type);
                    return true;
                }
            }
            return false;
        }

        #endregion Auxiliary Functions
    }
}
