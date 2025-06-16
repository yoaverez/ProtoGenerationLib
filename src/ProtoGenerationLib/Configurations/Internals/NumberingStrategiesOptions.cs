using ProtoGenerationLib.Configurations.Abstracts;

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
        public NumberingStrategiesOptions()
        {
            // Noting to do.
        }

        /// <summary>
        /// Create new instance of the <see cref="NumberingStrategiesOptions"/> class.
        /// </summary>
        /// <param name="fieldNumberingStrategy"><inheritdoc cref="FieldNumberingStrategy" path="/node()"/></param>
        /// <param name="enumValueNumberingStrategy"><inheritdoc cref="EnumValueNumberingStrategy" path="/node()"/></param>
        public NumberingStrategiesOptions(string fieldNumberingStrategy, string enumValueNumberingStrategy)
        {
            FieldNumberingStrategy = fieldNumberingStrategy;
            EnumValueNumberingStrategy = enumValueNumberingStrategy;
        }
    }
}
