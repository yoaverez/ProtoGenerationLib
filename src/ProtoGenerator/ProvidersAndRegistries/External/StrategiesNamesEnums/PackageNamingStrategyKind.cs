namespace ProtoGenerator.ProvidersAndRegistries.External.StrategiesNamesEnums
{
    /// <summary>
    /// Enum containing all the project defined
    /// package naming strategies.
    /// </summary>
    public enum PackageNamingStrategyKind
    {
        /// <summary>
        /// A package naming strategy where all types
        /// share the same package named "protos".
        /// </summary>
        SinglePackageNamedProtos,

        /// <summary>
        /// A package naming strategy where the package
        /// of a type is its namespace.
        /// </summary>
        NameSpaceAsPackageName,
    }
}
