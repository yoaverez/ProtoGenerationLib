using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerator.Models.Internals.IntermediateRepresentations;
using ProtoGenerator.Utilities;
using ProtoGenerator.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace ProtoGenerator.Strategies.Internals.FieldsAndPropertiesExtractionStrategies
{
    /// <summary>
    /// Utility functions for the fields and properties extraction strategies.
    /// </summary>
    internal static class FieldsAndPropertiesExtractionStrategiesUtils
    {
        /// <summary>
        /// Create <see cref="BindingFlags"/> from the given <paramref name="analysisOptions"/>.
        /// </summary>
        /// <param name="analysisOptions">The analysis options to create the <see cref="BindingFlags"/> from.</param>
        /// <returns>The <see cref="BindingFlags"/> according to the given <paramref name="analysisOptions"/>.</returns>
        public static BindingFlags CreateBindingFlags(IAnalysisOptions analysisOptions)
        {
            var bindingFlags = BindingFlags.Public | BindingFlags.Instance;

            if (analysisOptions.IncludePrivates)
                bindingFlags |= BindingFlags.NonPublic;

            if (analysisOptions.IncludeStatics)
                bindingFlags |= BindingFlags.Static;

            return bindingFlags;
        }

        /// <summary>
        /// Get all the potential names that will be considered as duplicate members
        /// (fields or properties) for the member with the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName">The member whose potential member names you want.</param>
        /// <returns>
        /// All the potential names that will be considered as duplicate members
        /// (fields or properties) for the member with the given <paramref name="memberName"/>.
        /// </returns>
        public static IEnumerable<string> GetPotentialDuplicateMemberNames(string memberName)
        {
            var UpperCamelCaseMemberName = memberName.ToUpperCamelCase();
            var potentialDuplicateFieldsNames = new string[]
            {
                UpperCamelCaseMemberName,
                $"_{UpperCamelCaseMemberName}",
                $"{char.ToLower(UpperCamelCaseMemberName[0])}{UpperCamelCaseMemberName.Substring(1)}",
                $"_{char.ToLower(UpperCamelCaseMemberName[0])}{UpperCamelCaseMemberName.Substring(1)}",
                $"<{UpperCamelCaseMemberName}>k__BackingField"
            };
            return potentialDuplicateFieldsNames;
        }

        /// <summary>
        /// Get the back field name if the given <paramref name="memberName"/>.
        /// </summary>
        /// <param name="memberName">The member whose back field name you want.</param>
        /// <returns>
        /// The back field name if the given <paramref name="memberName"/>.
        /// </returns>
        public static string GetBackFieldName(string memberName)
        {
            return $"<{memberName}>k__BackingField";
        }

        /// <summary>
        /// Try getting the fields and properties of the given <paramref name="type"/>
        /// from the first constructor with the given <paramref name="constructorAttribute"/>.
        /// </summary>
        /// <param name="type">The type whose fields and properties to get.</param>
        /// <param name="constructorAttribute">
        /// The type of the constructor attribute that means that all
        /// the constructor parameters are also all the
        /// <paramref name="type"/>s relevant fields and properties
        /// </param>
        /// <param name="fieldsAndProps">
        /// And enumerable for tuples. each tuple contains the field type
        /// and the field name.
        /// If there is no constructor that answers the demands,
        /// this will be an empty enumerable.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/> has
        /// a constructor with the given <paramref name="constructorAttribute"/>.
        /// </returns>
        public static bool TryGetFieldsAndPropertiesFromConstructor(Type type,
                                                                    Type constructorAttribute,
                                                                    out IEnumerable<IFieldMetadata> fieldsAndProps)
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            fieldsAndProps = new List<IFieldMetadata>();
            foreach (var ctor in type.GetConstructors(bindingFlags))
            {
                if (ctor.IsDefined(constructorAttribute, constructorAttribute.IsAttributeInherited()))
                {
                    var ctorParameters = ctor.GetParameters();
                    fieldsAndProps = ctorParameters.Select(param => new FieldMetadata(
                            type: param.ParameterType,
                            name: param.Name,
                            attributes: CustomAttributeExtensions.GetCustomAttributes(param, inherit: true).ToList(),
                            declaringType: type
                        )
                    ).ToArray();
                    return true;
                }
            }

            return false;
        }
    }
}
