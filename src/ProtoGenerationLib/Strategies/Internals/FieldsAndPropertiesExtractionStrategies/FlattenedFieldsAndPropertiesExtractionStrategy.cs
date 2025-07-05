using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Constants;
using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Strategies.Internals.DocumentationExtractionStrategies;
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
    /// Field and properties extraction strategy that flattened all the fields and property of the type.
    /// i.e. each field or property of base class or implemented interface will be taken as a single member.
    /// </summary>
    public class FlattenedFieldsAndPropertiesExtractionStrategy : IFieldsAndPropertiesExtractionStrategy
    {
        /// <inheritdoc/>
        public IEnumerable<IFieldMetadata> ExtractFieldsAndProperties(Type type,
                                                                      IAnalysisOptions analysisOptions,
                                                                      IDocumentationExtractionStrategy? documentationExtractionStrategy = null)
        {
            documentationExtractionStrategy = documentationExtractionStrategy ?? new NoDocumentationExtractionStrategy();
            return ExtractFieldsAndProperties(type, analysisOptions, documentationExtractionStrategy, new Dictionary<Type, bool> { [type] = false });
        }

        /// <param name="alreadyCheckedIsEmpty">
        /// Types that was already checked if they are empty.
        /// This is used to insure that recursive types won't loop forever.
        /// </param>
        /// <inheritdoc cref="ExtractFieldsAndProperties(Type, IAnalysisOptions, IDocumentationExtractionStrategy)"/>
        private IEnumerable<IFieldMetadata> ExtractFieldsAndProperties(Type type,
                                                                       IAnalysisOptions analysisOptions,
                                                                       IDocumentationExtractionStrategy documentationExtractionStrategy,
                                                                       Dictionary<Type, bool> alreadyCheckedIsEmpty)
        {
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
                var bindingFlags = CreateBindingFlags(analysisOptions);
                var namesToIgnore = new HashSet<string>();
                var props = ExtractProperties(type, bindingFlags, analysisOptions.IgnoreFieldOrPropertyAttribute, analysisOptions.DocumentationProvider, documentationExtractionStrategy, namesToIgnore);
                IEnumerable<IFieldMetadata> fields = new List<IFieldMetadata>();
                if (analysisOptions.IncludeFields)
                {
                    fields = ExtractFields(type, bindingFlags, analysisOptions.IgnoreFieldOrPropertyAttribute, analysisOptions.DocumentationProvider, documentationExtractionStrategy, namesToIgnore);
                }

                // Combine the fields and properties.
                fieldsAndProps = props.Concat(fields).ToList();
            }

            if (analysisOptions.RemoveEmptyMembers)
                // Remove all empty members that was not already analyzed
                // in order to prevent endless recursion.
                RemoveAllEmptyMembers(fieldsAndProps, analysisOptions, documentationExtractionStrategy, alreadyCheckedIsEmpty);

            return fieldsAndProps;
        }

        /// <summary>
        /// Extract all the properties of the given <paramref name="type"/>
        /// based on the given <paramref name="bindingFlags"/>.
        /// </summary>
        /// <param name="type">The type whose properties to extract.</param>
        /// <param name="bindingFlags">Binding flags to extract the wanted properties.</param>
        /// <param name="ignoreAttribute">The type of the attribute that says to ignore properties.</param>
        /// <param name="documentationProvider">A provider for documentation.</param>
        /// <param name="namesToIgnore">Names of properties to ignore.</param>
        /// <returns>An enumerable of fields meta datas.</returns>
        private IEnumerable<IFieldMetadata> ExtractProperties(Type type,
                                                              BindingFlags bindingFlags,
                                                              Type ignoreAttribute,
                                                              IDocumentationProvider documentationProvider,
                                                              IDocumentationExtractionStrategy documentationExtractionStrategy,
                                                              HashSet<string> namesToIgnore)
        {
            bindingFlags |= BindingFlags.DeclaredOnly;
            var properties = new List<IFieldMetadata>();

            var implementedInterfaces = type.GetAllImplementedInterfaces();
            foreach (var implementedInterface in implementedInterfaces)
            {
                foreach (var prop in implementedInterface.GetProperties(bindingFlags))
                {
                    // Make sure to deal only with properties that should not be ignored.
                    if (!prop.IsDefined(ignoreAttribute, ignoreAttribute.IsAttributeInherited()))
                    {
                        if (!namesToIgnore.Contains(prop.Name))
                        {
                            properties.Add(CreateFieldMetaDataFromPropertyInfo(type, prop, documentationProvider, documentationExtractionStrategy));
                            namesToIgnore.AddRange(GetPotentialDuplicateMemberNames(prop.Name));
                        }
                    }
                    else
                    {
                        // Make sure to ignore any back fields of the current field.
                        namesToIgnore.Add(GetBackFieldName(prop.Name));
                    }
                }
            }

            foreach (var prop in type.GetProperties(bindingFlags).OrderBy(prop => !prop.GetGetMethod(true).IsPublic))
            {
                // Make sure to deal only with properties that should not be ignored.
                if (!prop.IsDefined(ignoreAttribute, ignoreAttribute.IsAttributeInherited()))
                {
                    if (!namesToIgnore.Contains(prop.Name))
                    {
                        properties.Add(CreateFieldMetaDataFromPropertyInfo(type, prop, documentationProvider, documentationExtractionStrategy));
                        namesToIgnore.AddRange(GetPotentialDuplicateMemberNames(prop.Name));
                    }
                }
                else
                {
                    // Make sure to ignore any back fields of the current field.
                    namesToIgnore.Add(GetBackFieldName(prop.Name));
                }
            }

            if (type.TryGetBase(out var baseType))
            {
                var baseProps = ExtractProperties(baseType, bindingFlags, ignoreAttribute, documentationProvider, documentationExtractionStrategy, namesToIgnore);
                properties.AddRange(baseProps.Select(baseProp => new FieldMetadata(baseProp) { DeclaringType = type }).Cast<IFieldMetadata>().ToList());
            }
            return properties;
        }

        /// <summary>
        /// Extract all the fields of the given <paramref name="type"/>
        /// based on the given <paramref name="bindingFlags"/>.
        /// </summary>
        /// <param name="type">The type whose fields to extract.</param>
        /// <param name="bindingFlags">Binding flags to extract the wanted fields.</param>
        /// <param name="ignoreAttribute">The type of the attribute that says to ignore fields.</param>
        /// <param name="documentationProvider">A provider for documentation.</param>
        /// <param name="namesToIgnore">Names of fields to ignore.</param>
        /// <returns>An enumerable fields meta datas.</returns>
        private IEnumerable<IFieldMetadata> ExtractFields(Type type,
                                                          BindingFlags bindingFlags,
                                                          Type ignoreAttribute,
                                                          IDocumentationProvider documentationProvider,
                                                          IDocumentationExtractionStrategy documentationExtractionStrategy,
                                                          HashSet<string> namesToIgnore)
        {
            bindingFlags |= BindingFlags.DeclaredOnly;
            var fields = new List<IFieldMetadata>();

            foreach (var field in type.GetFields(bindingFlags).OrderBy(fieldInfo => !fieldInfo.IsPublic))
            {
                // Make sure to deal only with fields that should not be ignored.
                if (!field.IsDefined(ignoreAttribute, ignoreAttribute.IsAttributeInherited()))
                {
                    if (!namesToIgnore.Contains(field.Name))
                    {
                        fields.Add(CreateFieldMetaDataFromFieldInfo(type, field, documentationProvider, documentationExtractionStrategy));
                        namesToIgnore.AddRange(GetPotentialDuplicateMemberNames(field.Name));
                    }
                }
            }

            if (type.TryGetBase(out var baseType))
            {
                var baseFields = ExtractFields(baseType, bindingFlags, ignoreAttribute, documentationProvider, documentationExtractionStrategy, namesToIgnore);
                fields.AddRange(baseFields.Select(baseField => new FieldMetadata(baseField) { DeclaringType = type }).Cast<IFieldMetadata>().ToList());
            }

            return fields;
        }

        /// <summary>
        /// Remove all the empty members from the given <paramref name="fieldsAndProps"/>.
        /// Empty members are members without fields and properties.
        /// </summary>
        /// <param name="fieldsAndProps">The fields and properties to remove items from.</param>
        /// <param name="analysisOptions">The analysis options.</param>
        /// <param name="alreadyCheckedIsEmpty">Types that was already checked if they are empty.</param>
        private void RemoveAllEmptyMembers(List<IFieldMetadata> fieldsAndProps, IAnalysisOptions analysisOptions, IDocumentationExtractionStrategy documentationExtractionStrategy, Dictionary<Type, bool> alreadyCheckedIsEmpty)
        {
            var typesToCheck = fieldsAndProps.Select(memberMetadata => memberMetadata.Type)
                                             .Where(memberType => !alreadyCheckedIsEmpty.ContainsKey(memberType))
                                             .ToHashSet();

            // Remove all types that were already checked and are empty.
            var typesToRemove = alreadyCheckedIsEmpty.Where(typeIsChecked => typeIsChecked.Value)
                                                     .Select(typeIsChecked => typeIsChecked.Key)
                                                     .ToHashSet();
            foreach (var typeToCheck in typesToCheck)
            {
                // The typeToCheck may already been check be previous iteration of this loop.
                // So only check again if needed.
                var isEmpty = false;
                if (!alreadyCheckedIsEmpty.TryGetValue(typeToCheck, out isEmpty))
                {
                    alreadyCheckedIsEmpty.Add(typeToCheck, false);
                    if (IsTypeEmpty(typeToCheck, analysisOptions, documentationExtractionStrategy, alreadyCheckedIsEmpty))
                    {
                        alreadyCheckedIsEmpty[typeToCheck] = true;
                        isEmpty = true;
                    }
                }

                if (isEmpty)
                    typesToRemove.Add(typeToCheck);
            }

            fieldsAndProps.RemoveAll(member => typesToRemove.Contains(member.Type));
        }

        /// <summary>
        /// Checks whether or not the given <paramref name="type"/> is an empty type
        /// i.e. type that contains no fields and properties.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="analysisOptions">The analysis options.</param>
        /// <param name="alreadyCheckedIsEmpty">Types that was already checked if they are empty.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/> is empty,
        /// otherwise <see langword="false"/>.
        /// </returns>
        private bool IsTypeEmpty(Type type, IAnalysisOptions analysisOptions, IDocumentationExtractionStrategy documentationExtractionStrategy, Dictionary<Type, bool> alreadyCheckedIsEmpty)
        {
            // Not a well known type and also does not have fields or properties.
            return !WellKnownTypesConstants.WellKnownTypes.ContainsKey(type)
                && !type.IsEnum
                && !type.IsEnumerableType()
                && !ExtractFieldsAndProperties(type, analysisOptions, documentationExtractionStrategy, alreadyCheckedIsEmpty).Any();
        }

        /// <summary>
        /// Create a <see cref="FieldMetadata"/> from the given <paramref name="prop"/>.
        /// </summary>
        /// <param name="type">The type that declare the given <paramref name="prop"/>.</param>
        /// <param name="prop">The property to convert to <see cref="FieldMetadata"/>.</param>
        /// <param name="documentationProvider">A provider for user defined documentation.</param>
        /// <param name="documentationExtractionStrategy">An extractor for csharp entities documentation.</param>
        /// <returns>
        /// A new <see cref="IFieldMetadata"/> that represent the given <paramref name="prop"/>.
        /// </returns>
        private IFieldMetadata CreateFieldMetaDataFromPropertyInfo(Type type,
                                                                   PropertyInfo prop,
                                                                   IDocumentationProvider documentationProvider,
                                                                   IDocumentationExtractionStrategy documentationExtractionStrategy)
        {
            var fieldMetadata = new FieldMetadata
            (
                type: prop.PropertyType,
                name: prop.Name,
                attributes: CustomAttributeExtensions.GetCustomAttributes(prop, true).ToList(),
                declaringType: type
            );

            if(TryGetPropertyDocumentation(type, prop, documentationProvider, documentationExtractionStrategy, out var documentation))
                fieldMetadata.Documentation = documentation;

            return fieldMetadata;
        }

        /// <summary>
        /// Create a <see cref="FieldMetadata"/> from the given <paramref name="fieldInfo"/>.
        /// </summary>
        /// <param name="type">The type that declare the given <paramref name="fieldInfo"/>.</param>
        /// <param name="fieldInfo">The field to convert to <see cref="FieldMetadata"/>.</param>
        /// <param name="documentationProvider">A provider for user defined documentation.</param>
        /// <param name="documentationExtractionStrategy">An extractor for csharp entities documentation.</param>
        /// <returns>
        /// A new <see cref="IFieldMetadata"/> that represent the given <paramref name="fieldInfo"/>.
        /// </returns>
        private IFieldMetadata CreateFieldMetaDataFromFieldInfo(Type type,
                                                                FieldInfo fieldInfo,
                                                                IDocumentationProvider documentationProvider,
                                                                IDocumentationExtractionStrategy documentationExtractionStrategy)
        {
            var fieldMetadata = new FieldMetadata
            (
                type: fieldInfo.FieldType,
                name: fieldInfo.Name,
                attributes: CustomAttributeExtensions.GetCustomAttributes(fieldInfo, true).ToList(),
                declaringType: type
            );

            if (TryGetFieldDocumentation(type, fieldInfo, documentationProvider, documentationExtractionStrategy, out var documentation))
                fieldMetadata.Documentation = documentation;

            return fieldMetadata;
        }

        /// <summary>
        /// Try getting the documentation of the given <paramref name="prop"/>.
        /// </summary>
        /// <param name="type">The type that declare the given <paramref name="prop"/>.</param>
        /// <param name="prop">The property whose documentation is requested.</param>
        /// <param name="documentationProvider">A provider for user defined documentation.</param>
        /// <param name="documentationExtractionStrategy">An extractor for csharp entities documentation.</param>
        /// <param name="documentation">The documentation if found.</param>
        /// <returns>
        /// <see langword="true"/> if the documentation of the given <paramref name="prop"/>
        /// was found otherwise <see langword="false"/>.
        /// </returns>
        private bool TryGetPropertyDocumentation(Type type,
                                                 PropertyInfo prop,
                                                 IDocumentationProvider documentationProvider,
                                                 IDocumentationExtractionStrategy documentationExtractionStrategy,
                                                 out string documentation)
        {
            if (!documentationProvider.TryGetFieldDocumentation(type, prop.Name, out documentation))
            {
                if (!documentationExtractionStrategy.TryGetPropertyDocumentation(prop, out documentation))
                {
                    documentation = string.Empty;
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Try getting the documentation of the given <paramref name="fieldInfo"/>.
        /// </summary>
        /// <param name="type">The type that declare the given <paramref name="fieldInfo"/>.</param>
        /// <param name="fieldInfo">The field whose documentation is requested.</param>
        /// <param name="documentationProvider">A provider for user defined documentation.</param>
        /// <param name="documentationExtractionStrategy">An extractor for csharp entities documentation.</param>
        /// <param name="documentation">The documentation if found.</param>
        /// <returns>
        /// <see langword="true"/> if the documentation of the given <paramref name="fieldInfo"/>
        /// was found otherwise <see langword="false"/>.
        /// </returns>
        private bool TryGetFieldDocumentation(Type type,
                                              FieldInfo fieldInfo,
                                              IDocumentationProvider documentationProvider,
                                              IDocumentationExtractionStrategy documentationExtractionStrategy,
                                              out string documentation)
        {
            if (!documentationProvider.TryGetFieldDocumentation(type, fieldInfo.Name, out documentation))
            {
                if (!documentationExtractionStrategy.TryGetFieldDocumentation(fieldInfo, out documentation))
                {
                    documentation = string.Empty;
                    return false;
                }
            }

            return true;
        }
    }
}
