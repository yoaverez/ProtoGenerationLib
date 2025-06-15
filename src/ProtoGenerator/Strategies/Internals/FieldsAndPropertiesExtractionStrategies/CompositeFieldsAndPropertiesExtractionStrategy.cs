using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Internals.IntermediateRepresentations;
using ProtoGenerator.Strategies.Abstracts;
using ProtoGenerator.Utilities;
using ProtoGenerator.Utilities.CollectionUtilities;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static ProtoGenerator.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.FieldsAndPropertiesExtractionStrategiesUtils;

namespace ProtoGenerator.Strategies.Internals.FieldsAndPropertiesExtractionStrategies
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
        public IEnumerable<IFieldMetadata> ExtractFieldsAndProperties(Type type, IAnalysisOptions analysisOptions)
        {
            var fieldsAndProps = new List<IFieldMetadata>();
            if (TryGetFieldsAndPropertiesFromConstructor(type, analysisOptions.DataTypeConstructorAttribute, out var constructorFields))
            {
                // There is a constructor tell tells all the
                // important fields and properties of the given type.
                fieldsAndProps = constructorFields.ToList();
            }
            else
            {
                // Get the none empty base type if it exists.
                var baseTypes = GetNoneEmptyBaseType(type, analysisOptions, out var baseTypeMembersNames);

                // Convert the base types to members i.e. type and name.
                var baseTypesAsMembers = baseTypes.Select(baseType => new FieldMetadata
                 (
                    type: baseType,
                    name: baseType.Name.ToUpperCamelCase(),
                    attributes: CustomAttributeExtensions.GetCustomAttributes(baseType, inherit: true),
                    declaringType : type
                 )).Cast<IFieldMetadata>().ToList();

                // Ignore all names of members that are contained in the base type.
                var namesToIgnore = baseTypeMembersNames.SelectMany(GetPotentialDuplicateMemberNames).ToHashSet();

                // Ignore all names of members with the same name as the base type.
                namesToIgnore.AddRange(baseTypesAsMembers.SelectMany(baseTypeMember => GetPotentialDuplicateMemberNames(baseTypeMember.Name)));

                // Get all the members of this type.
                var allMembers = flattenedMembersStrategy.ExtractFieldsAndProperties(type, analysisOptions);

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
        /// Get all the none empty base types (there can be either 0 or 1)
        /// of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose base type to get.</param>
        /// <param name="analysisOptions">The analysis options.</param>
        /// <param name="baseTypeMembersNames">Will contain the names of all the base type members.</param>
        /// <returns>
        /// All the none empty base types (there can be either 0 or 1)
        /// of the given <paramref name="type"/>.
        /// </returns>
        private IEnumerable<Type> GetNoneEmptyBaseType(Type type, IAnalysisOptions analysisOptions, out IEnumerable<string> baseTypeMembersNames)
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
            }

            baseTypeMembersNames = membersNames;
            return result;
        }
    }
}
