using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Converters.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.Models.Internals.ProtoDefinitions;
using ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Utilities.CollectionUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib.Converters.Internals.IntermediateToProtoDefinition
{
    /// <summary>
    /// A converter between enum intermediate representation to its proto representation.
    /// </summary>
    public class EnumTypeMetadataToEnumDefinitionConverter : IIntermediateToProtoDefinitionConverter<IEnumTypeMetadata, IEnumDefinition>
    {
        /// <summary>
        /// A provider of all the proto generator customizations.
        /// </summary>
        private IProvider componentsProvider;

        /// <summary>
        /// Constant represents a enum name of "UNKNOWN".
        /// </summary>
        private const string ENUM_NAME_UNKNOWN = "UNKNOWN";

        /// <summary>
        /// Constant represents a enum name of "UNSPECIFIED".
        /// </summary>
        private const string ENUM_NAME_UNSPECIFIED = "UNSPECIFIED";

        /// <summary>
        /// Constant represents a enum name of "THERE_IS_NO_FUCKING_WAY_THAT_YOU_HAVE_THIS_ENUM_NAME".
        /// </summary>
        private const string ENUM_NAME_IMPOSSIBLE = "THERE_IS_NO_FUCKING_WAY_THAT_YOU_HAVE_THIS_ENUM_NAME";

        /// <summary>
        /// Create new instance of the <see cref="EnumTypeMetadataToEnumDefinitionConverter"/> class.
        /// </summary>
        /// <param name="componentsProvider"><inheritdoc cref="componentsProvider" path="/node()"/></param>
        public EnumTypeMetadataToEnumDefinitionConverter(IProvider componentsProvider)
        {
            this.componentsProvider = componentsProvider;
        }

        /// <inheritdoc/>
        public IEnumDefinition ConvertIntermediateRepresentationToProtoDefinition(IEnumTypeMetadata intermediateType,
                                                                                  IReadOnlyDictionary<Type, IProtoTypeMetadata> protoTypesMetadatas,
                                                                                  IProtoGenerationOptions generationOptions)
        {
            var protoMetadata = protoTypesMetadatas[intermediateType.Type];

            var enumValueNumberingStrategy = componentsProvider.GetEnumValueNumberingStrategy(generationOptions.NumberingStrategiesOptions.EnumValueNumberingStrategy);
            var enumValueStylingStrategy = componentsProvider.GetProtoStylingStrategy(generationOptions.ProtoStylingConventionsStrategiesOptions.EnumValueStylingStrategy);

            var enumValueDefinitions = new List<EnumValueDefinition>();
            int i = 0;
            int numOfValues = intermediateType.Values.Count();
            foreach (var enumValue in intermediateType.Values)
            {
                var enumValueName = enumValueStylingStrategy.ToProtoStyle($"{protoMetadata.Name}_{enumValue.Name}");
                var enumValueNumber = enumValueNumberingStrategy.GetEnumValueNumber(intermediateType, enumValue, i, numOfValues);
                enumValueDefinitions.Add(new EnumValueDefinition(enumValueName, enumValueNumber));
                i++;
            }

            EnsureZeroValueFirst(ref enumValueDefinitions, protoMetadata.Name!, enumValueStylingStrategy, intermediateType.Type);
            ThrowIfThereAreEnumValuesWithTheSameEnumValue(intermediateType.Type, enumValueDefinitions);
            return new EnumDefinition(protoMetadata.Name!, protoMetadata.Package!, enumValueDefinitions.Cast<IEnumValueDefinition>());
        }

        /// <summary>
        /// Insure that the enum value of zero exists and placed first.
        /// If there is no enum value with the value of zero, one will be created.
        /// </summary>
        /// <param name="enumValueDefinitions">The enum values to modify.</param>
        /// <param name="enumName">The name of the enum.</param>
        /// <param name="enumValueStylingStrategy">The enum value styling strategy.</param>
        /// <param name="enumType">The type of the enum.</param>
        /// <exception cref="Exception">
        /// Thrown when there is no enum value with the value zero
        /// and a legal name for a new zero enum value was not found.
        /// </exception>
        private void EnsureZeroValueFirst(ref List<EnumValueDefinition> enumValueDefinitions, string enumName, IProtoStylingStrategy enumValueStylingStrategy, Type enumType)
        {
            var zeroValue = enumValueDefinitions.FirstOrDefault(enumValue => enumValue.Value == 0);
            if (zeroValue is not null)
            {
                enumValueDefinitions.Remove(zeroValue);
            }
            else
            {
                // Need to create zero value.
                var unavailableNames = enumValueDefinitions.Select(x => x.Name).ToHashSet();
                var unknownName = enumValueStylingStrategy.ToProtoStyle($"{enumName}_{ENUM_NAME_UNKNOWN}");
                var unspecifiedName = enumValueStylingStrategy.ToProtoStyle($"{enumName}_{ENUM_NAME_UNSPECIFIED}");
                var impossibleName = enumValueStylingStrategy.ToProtoStyle($"{enumName}_{ENUM_NAME_IMPOSSIBLE}");
                if (!unavailableNames.Contains(unknownName))
                {
                    zeroValue = new EnumValueDefinition(unknownName, 0);
                }
                else if (!unavailableNames.Contains(unspecifiedName))
                {
                    zeroValue = new EnumValueDefinition(unspecifiedName, 0);
                }
                else if (!unavailableNames.Contains(impossibleName))
                {
                    zeroValue = new EnumValueDefinition(impossibleName, 0);
                }
                else
                {
                    throw new Exception($"Your enum: {enumType.Name} does not contains a value " +
                        $"of 0 and some how contains values for" +
                        $"\"UNKNOWN\", \"UNSPECIFIED\" and \"THERE_IS_NO_FUCKING_WAY_THAT_YOU_HAVE_THIS_ENUM_NAME\"");
                }
            }

            enumValueDefinitions.Insert(0, zeroValue!);
        }

        /// <summary>
        /// Throw an exception if there are two enum values that share the same numeric value.
        /// </summary>
        /// <param name="enumType">The type of the enum.</param>
        /// <param name="enumValueDefinitions">The values of the enum.</param>
        /// <exception cref="Exception">Thrown when there are two enum values that share the same numeric value.</exception>
        private void ThrowIfThereAreEnumValuesWithTheSameEnumValue(Type enumType, IEnumerable<EnumValueDefinition> enumValueDefinitions)
        {
            var enumValues = new Dictionary<int, string>();
            foreach (var enumValue in enumValueDefinitions)
            {
                if (enumValues.ContainsKey(enumValue.Value))
                {
                    throw new Exception($"The enum type: {enumType.Name} have two values with different name " +
                        $"but the same numeric value." +
                        $"The names are {enumValue.Name} and {enumValues[enumValue.Value]} " +
                        $"and the numeric value is {enumValue.Value}.");
                }
                enumValues.Add(enumValue.Value, enumValue.Name);
            }
        }
    }
}
