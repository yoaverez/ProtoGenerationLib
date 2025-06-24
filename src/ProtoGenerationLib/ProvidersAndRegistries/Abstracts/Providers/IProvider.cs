namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider that provide all the customizations.
    /// </summary>
    internal interface IProvider : ICustomFieldSuffixesProvider,
                                   INumberingStrategiesProvider,
                                   IProtoNamingStrategiesProvider,
                                   IProtoStylingConventionsStrategiesProvider,
                                   IExtractionStrategiesProvider,
                                   INewTypeNamingStrategiesProvider

    {
        // Noting to do.
    }
}
