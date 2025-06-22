namespace ProtoGenerationLib.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider that provide all the customizations.
    /// </summary>
    public interface IProvider : ICustomConvertersProvider,
                                 ICustomTypeMappersProvider,
                                 ICustomFieldSuffixesProvider,
                                 INumberingStrategiesProvider,
                                 IProtoNamingStrategiesProvider,
                                 IProtoStylingConventionsStrategiesProvider,
                                 IExtractionStrategiesProvider,
                                 INewTypeNamingStrategiesProvider

    {
        // Noting to do.
    }
}
