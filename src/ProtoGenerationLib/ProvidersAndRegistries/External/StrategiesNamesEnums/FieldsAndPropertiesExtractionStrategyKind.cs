namespace ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums
{
    /// <summary>
    /// Enum containing all the project defined fields
    /// and properties extraction strategies.
    /// </summary>
    public enum FieldsAndPropertiesExtractionStrategyKind
    {
        /// <summary>
        /// Field and properties extraction strategy that
        /// composite base types to single fields.
        /// </summary>
        Composite,

        /// <summary>
        /// Field and properties extraction strategy that
        /// flattened all the fields and property of the type.
        /// i.e. each field or property of base class or
        /// implemented interface will be taken as a single member.
        /// </summary>
        Flatten,
    }
}
