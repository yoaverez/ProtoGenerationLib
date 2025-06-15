namespace ProtoGenerator.ProvidersAndRegistries.External.StrategiesNamesEnums
{
    /// <summary>
    /// Enum containing all the project defined
    /// file path strategies.
    /// </summary>
    public enum FilePathStrategyKind
    {
        /// <summary>
        /// A file path strategy that groups types by their namespace.
        /// </summary>
        NameSpace,

        /// <summary>
        /// A file path strategy that puts all types in a single
        /// proto file named "protos".
        /// </summary>
        SingleFileNamedProtos,

        /// <summary>
        /// A file path strategy that
        /// in which the file name will be the type name.
        /// </summary>
        TypeName,
    }
}
