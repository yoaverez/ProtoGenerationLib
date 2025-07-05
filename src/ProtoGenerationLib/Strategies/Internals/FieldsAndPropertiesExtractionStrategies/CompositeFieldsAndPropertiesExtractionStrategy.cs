using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Strategies.Internals.DocumentationExtractionStrategies;
using ProtoGenerationLib.Utilities;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static ProtoGenerationLib.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.FieldsAndPropertiesExtractionStrategiesUtils;

namespace ProtoGenerationLib.Strategies.Internals.FieldsAndPropertiesExtractionStrategies
{
    /// <summary>
    /// Field and properties extraction strategy that composite base types
    /// to a single fields.
    /// </summary>
    public class CompositeFieldsAndPropertiesExtractionStrategy : IFieldsAndPropertiesExtractionStrategy
    {
        /// <summary>
        /// Extraction strategy that extract all the fields and properties in
        /// a flatten matter.
        /// </summary>
        private IFieldsAndPropertiesExtractionStrategy flattenedMembersStrategy;

        /// <summary>
        /// Create new instance of the <see cref="CompositeFieldsAndPropertiesExtractionStrategy"/> class.
        /// </summary>
        /// <param name="flattenedMembersStrategy"><inheritdoc cref="flattenedMembersStrategy" path="/node()"/></param>
        public CompositeFieldsAndPropertiesExtractionStrategy(IFieldsAndPropertiesExtractionStrategy? flattenedMembersStrategy = null)
        {
            this.flattenedMembersStrategy = flattenedMembersStrategy ?? new FlattenedFieldsAndPropertiesExtractionStrategy();
        }

        /// <inheritdoc/>
        public IEnumerable<IFieldMetadata> ExtractFieldsAndProperties(Type type,
                                                                      IAnalysisOptions analysisOptions,
                                                                      IDocumentationExtractionStrategy? documentationExtractionStrategy = null)
        {
            documentationExtractionStrategy = documentationExtractionStrategy ?? new NoDocumentationExtractionStrategy();
            var fieldsAndProps = new List<IFieldMetadata>();
            if (TryGetFieldsAndPropertiesFromConstructor(type,
                                                         analysisOptions.DataTypeConstructorAttribute,
                                                         analysisOptions.DocumentationProvider,
                                                         documentationExtractionStrategy,
                                                         out var constructorFields))
            {
                // There is a constructor tell tells all the
                // important fields and properties of the given type.
                fieldsAndProps = constructorFields.ToList();
            }
            else
            {
                // Get the none empty base type if it exists.
                var baseTypes = GetBaseType(type, analysisOptions, out var baseTypeMembersNames);

                // Convert the base types to members i.e. type and name.
                var baseTypesAsMembers = baseTypes.Select(baseType =>
                {
                    var fieldMetadata = new FieldMetadata
                    (
                       type: baseType,
                       name: baseType.Name.ToUpperCamelCase(),
                       attributes: CustomAttributeExtensions.GetCustomAttributes(baseType, inherit: true),
                       declaringType: type
                    );

                    if (TryGetBaseTypeFieldDocumentation(type, baseType, analysisOptions.DocumentationProvider, documentationExtractionStrategy, out var documentation))
                        fieldMetadata.Documentation = documentation;

                    return fieldMetadata;
                 }).Cast<IFieldMetadata>().ToList();

                // Ignore all names of members that are contained in the base type.
                var namesToIgnore = baseTypeMembersNames.SelectMany(GetPotentialDuplicateMemberNames).ToHashSet();

                // Ignore all names of members with the same name as the base type.
                namesToIgnore.AddRange(baseTypesAsMembers.SelectMany(baseTypeMember => GetPotentialDuplicateMemberNames(baseTypeMember.Name)));

                // Get all the members of this type.
                var allMembers = flattenedMembersStrategy.ExtractFieldsAndProperties(type, analysisOptions, documentationExtractionStrategy);

                // Initialize the fields and properties of
                // this type with the base type as a field.
                fieldsAndProps = baseTypesAsMembers;

                // Add to the fields and properties of
                // this type only unique members.
                foreach (var member in allMembers)
                {
                    if (!namesToIgnore.Contains(member.Name))
                    {
                        var newMember = new FieldMetadata(member)
                        {
                            DeclaringType = type
                        };
                        fieldsAndProps.Add(member);
                    }
                }
            }

            return fieldsAndProps;
        }

        /// <summary>
        /// Get all the base types (there can be either 0 or 1)
        /// of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose base type to get.</param>
        /// <param name="analysisOptions">The analysis options.</param>
        /// <param name="baseTypeMembersNames">Will contain the names of all the base type members.</param>
        /// <returns>
        /// All the base types (there can be either 0 or 1)
        /// of the given <paramref name="type"/>.
        /// </returns>
        private IEnumerable<Type> GetBaseType(Type type, IAnalysisOptions analysisOptions, out IEnumerable<string> baseTypeMembersNames)
        {
            var result = new HashSet<Type>();
            var membersNames = new HashSet<string>();
            if (type.TryGetBase(out var baseType))
            {
                var members = flattenedMembersStrategy.ExtractFieldsAndProperties(baseType, analysisOptions);
                if (members.Any())
                {
                    result.Add(baseType);
                    membersNames.AddRange(members.Select(member => member.Name));
                }
                else
                {
                    // There are no members meaning that the base type is empty.
                    // So if empty members should not be removed,
                    // Add the empty base type.
                    if (!analysisOptions.RemoveEmptyMembers)
                        result.Add(baseType);
                }
            }

            baseTypeMembersNames = membersNames;
            return result;
        }

        /// <summary>
        /// Try getting the documentation of the given <paramref name="baseType"/>
        /// as a field.
        /// </summary>
        /// <param name="subClassType">The type that declare the given <paramref name="prop"/>.</param>
        /// <param name="baseType">The property whose documentation is requested.</param>
        /// <param name="documentationProvider">A provider for user defined documentation.</param>
        /// <param name="documentationExtractionStrategy">An extractor for csharp entities documentation.</param>
        /// <param name="documentation">The documentation if found.</param>
        /// <returns>
        /// <see langword="true"/> if the documentation of the given <paramref name="baseType"/>
        /// as a field was found otherwise <see langword="false"/>.
        /// </returns>
        private bool TryGetBaseTypeFieldDocumentation(Type subClassType,
                                                      Type baseType,
                                                      IDocumentationProvider documentationProvider,
                                                      IDocumentationExtractionStrategy documentationExtractionStrategy,
                                                      out string documentation)
        {
            if (!documentationProvider.TryGetFieldDocumentation(subClassType, baseType.Name, out documentation))
            {
                if (!documentationExtractionStrategy.TryGetBaseTypeFieldDocumentation(subClassType, baseType, out documentation))
                {
                    documentation = string.Empty;
                    return false;
                }
            }

            return true;
        }
    }
}
