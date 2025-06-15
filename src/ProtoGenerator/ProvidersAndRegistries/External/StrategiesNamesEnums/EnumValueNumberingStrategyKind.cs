namespace ProtoGenerator.ProvidersAndRegistries.External.StrategiesNamesEnums
{
    /// <summary>
    /// Enum containing all the project defined enum value
    /// numbering strategies.
    /// </summary>
    public enum EnumValueNumberingStrategyKind
    {
        /// <summary>
        /// Enum value numbering strategy that assigns
        /// the same value of the enum value as the enum value.
        /// </summary>
        SameAsEnumValue,

        /// <summary>
        /// Enum value numbering strategy that assigns the index
        /// of the enum value as the enum value.
        /// </summary>
        Sequential,
    }
}
