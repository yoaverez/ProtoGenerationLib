using ProtoGenerator.Configurations.Abstracts;

namespace ProtoGenerator.Configurations.Internals
{
    /// <inheritdoc cref="INumberingStrategiesOptions"/>
    public class NumberingStrategiesOptions : INumberingStrategiesOptions
    {
        /// <inheritdoc/>
        public string FieldNumberingStrategy { get; set; }

        /// <inheritdoc/>
        public string EnumValueNumberingStrategy { get; set; }
    }
}
