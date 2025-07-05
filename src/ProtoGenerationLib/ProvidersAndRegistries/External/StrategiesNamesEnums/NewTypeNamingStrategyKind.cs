namespace ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums
{
    /// <summary>
    /// Enum containing all the project defined
    /// new type naming strategies.
    /// </summary>
    public enum NewTypeNamingStrategyKind
    {
        /// <summary>
        /// Naming strategy for new types that used the name of the type if
        /// the type is not generic otherwise uses the name without generics
        /// and a combination of the generic arguments names.
        /// </summary>
        NameAsAlphaNumericTypeName,
    }
}
