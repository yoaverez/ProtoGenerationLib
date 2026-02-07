namespace ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums
{
    /// <summary>
    /// Enum containing all the project defined method signature extraction strategies.
    /// </summary>
    public enum MethodSignatureExtractionStrategyKind
    {
        /// <summary>
        /// Method signature extraction strategy that
        /// ignore method parameters that have an ignore attribute,
        /// and replaces types that do not make any since in rpc
        /// like the Task type.
        /// </summary>
        Default,

        /// <summary>
        /// Method signature extraction strategy that
        /// ignore method parameters that appears in grpc client
        /// but should not appear on the rpc itself
        /// like header, deadline and cancellation token.
        /// </summary>
        /// <remarks>
        /// This should be used if the csharp type that
        /// defines a proto service has at least one of the above parameters
        /// in method parameters.
        /// </remarks>
        ResembleProtoClientMethod,
    }
}
