using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;
using ProtoGenerationLib.ProvidersAndRegistries.External;

namespace ProtoGenerationLib.Configurations.Internals
{
    /// <inheritdoc cref="INumberingStrategiesOptions"/>
    public class NumberingStrategiesOptions : INumberingStrategiesOptions
    {
        /// <inheritdoc/>
        public string FieldNumberingStrategy { get; set; }

        /// <inheritdoc/>
        public string EnumValueNumberingStrategy { get; set; }

        /// <summary>
        /// Create new instance of the <see cref="NumberingStrategiesOptions"/> class.
        /// </summary>
        /// <param name="fieldNumberingStrategy"><inheritdoc cref="FieldNumberingStrategy" path="/node()"/><br/> Default to null converted to "Sequential".</param>
        /// <param name="enumValueNumberingStrategy"><inheritdoc cref="EnumValueNumberingStrategy" path="/node()"/><br/> Default to null converted to "Sequential".</param>
        public NumberingStrategiesOptions(string? fieldNumberingStrategy = null,
                                          string? enumValueNumberingStrategy = null)
        {
            FieldNumberingStrategy = fieldNumberingStrategy ??
                StrategyNamesLookup.FieldNumberingStrategiesLookup[FieldNumberingStrategyKind.Sequential];
            EnumValueNumberingStrategy = enumValueNumberingStrategy ??
                StrategyNamesLookup.EnumValueNumberingStrategiesLookup[EnumValueNumberingStrategyKind.Sequential];
        }
    }
}
