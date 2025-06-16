using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static ProtoGenerationLib.Strategies.Internals.FieldsAndPropertiesExtractionStrategies.FieldsAndPropertiesExtractionStrategiesUtils;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using ProtoGenerationLib.Constants;

namespace ProtoGenerationLib.Strategies.Internals.FieldsAndPropertiesExtractionStrategies
{
    /// <summary>
    /// Field and properties extraction strategy that flattened all the fields and property of the type.
    /// i.e. each field or property of base class or implemented interface will be taken as a single member.
    /// </summary>
    public class FlattenedFieldsAndPropertiesExtractionStrategy : IFieldsAndPropertiesExtractionStrategy
    {
        /// <inheritdoc/>
        public IEnumerable<IFieldMetadata> ExtractFieldsAndProperties(Type type, IAnalysisOptions analysisOptions)
        {
            return ExtractFieldsAndProperties(type, analysisOptions, new Dictionary<Type, bool> { [type] = false });
        }

        /// <param name="alreadyCheckedIsEmpty">
        /// Types that was already checked if they are empty.
        /// This is used to insure that recursive types won't loop forever.
        /// </param>
        /// <inheritdoc cref="ExtractFieldsAndProperties(Type, IAnalysisOptions)"/>
        private IEnumerable<IFieldMetadata> ExtractFieldsAndProperties(Type type, IAnalysisOptions analysisOptions, Dictionary<Type, bool> alreadyCheckedIsEmpty)
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
                var bindingFlags = CreateBindingFlags(analysisOptions);
                var namesToIgnore = new HashSet<string>();
                var props = ExtractProperties(type, bindingFlags, analysisOptions.IgnoreFieldOrPropertyAttribute, namesToIgnore);
                IEnumerable<IFieldMetadata> fields = new List<IFieldMetadata>();
                if (analysisOptions.IncludeFields)
                {
                    fields = ExtractFields(type, bindingFlags, analysisOptions.IgnoreFieldOrPropertyAttribute, namesToIgnore);
                }

                // Combine the fields and properties.
                fieldsAndProps = props.Concat(fields).ToList();
            }

            // Remove all empty members that was not already analyzed
            // in order to prevent endless recursion.
            RemoveAllEmptyMembers(fieldsAndProps, analysisOptions, alreadyCheckedIsEmpty);

            return fieldsAndProps;
        }

        /// <summary>
        /// Extract all the properties of the given <paramref name="type"/>
        /// based on the given <paramref name="bindingFlags"/>.
        /// </summary>
        /// <param name="type">The type whose properties to extract.</param>
        /// <param name="bindingFlags">Binding flags to extract the wanted properties.</param>
        /// <param name="ignoreAttribute">The type of the attribute that says to ignore properties.</param>
        /// <param name="namesToIgnore">Names of properties to ignore.</param>
        /// <returns>An enumerable of fields meta datas.</returns>
        private IEnumerable<IFieldMetadata> ExtractProperties(Type type, BindingFlags bindingFlags, Type ignoreAttribute, HashSet<string> namesToIgnore)
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
                            properties.Add(new FieldMetadata
                            (
                                type: prop.PropertyType,
                                name: prop.Name,
                                attributes: CustomAttributeExtensions.GetCustomAttributes(prop, true).ToList(),
                                declaringType: type
                            ));
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
                        properties.Add(new FieldMetadata
                        (
                            type: prop.PropertyType,
                            name: prop.Name,
                            attributes: CustomAttributeExtensions.GetCustomAttributes(prop, true).ToList(),
                            declaringType: type
                        ));
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
                var baseProps = ExtractProperties(baseType, bindingFlags, ignoreAttribute, namesToIgnore);
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
        /// <param name="namesToIgnore">Names of fields to ignore.</param>
        /// <returns>An enumerable fields meta datas.</returns>
        private IEnumerable<IFieldMetadata> ExtractFields(Type type, BindingFlags bindingFlags, Type ignoreAttribute, HashSet<string> namesToIgnore)
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
                        fields.Add(new FieldMetadata
                            (
                                type: field.FieldType,
                                name: field.Name,
                                attributes: CustomAttributeExtensions.GetCustomAttributes(field, true).ToList(),
                                declaringType: type
                            ));
                        namesToIgnore.AddRange(GetPotentialDuplicateMemberNames(field.Name));
                    }
                }
            }

            if (type.TryGetBase(out var baseType))
            {
                var baseFields = ExtractFields(baseType, bindingFlags, ignoreAttribute, namesToIgnore);
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
        private void RemoveAllEmptyMembers(List<IFieldMetadata> fieldsAndProps, IAnalysisOptions analysisOptions, Dictionary<Type, bool> alreadyCheckedIsEmpty)
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
                alreadyCheckedIsEmpty.Add(typeToCheck, false);
                if (IsTypeEmpty(typeToCheck, analysisOptions, alreadyCheckedIsEmpty))
                {
                    alreadyCheckedIsEmpty[typeToCheck] = true;
                    typesToRemove.Add(typeToCheck);
                };
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
        private bool IsTypeEmpty(Type type, IAnalysisOptions analysisOptions, Dictionary<Type, bool> alreadyCheckedIsEmpty)
        {
            // Not a well known type and also does not have fields or properties.
            return !WellKnownTypesConstants.WellKnownTypes.ContainsKey(type) && !ExtractFieldsAndProperties(type, analysisOptions, alreadyCheckedIsEmpty).Any();
        }
    }
}
