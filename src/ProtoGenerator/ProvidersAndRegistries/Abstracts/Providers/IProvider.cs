namespace ProtoGenerator.ProvidersAndRegistries.Abstracts.Providers
{
    /// <summary>
    /// Provider that provide all the customizations.
    /// </summary>
    public interface IProvider : ICustomConvertersProvider,
                                 ICustomTypeNameMappersProvider,
                                 INumberingStrategiesProvider,
                                 IProtoNamingStrategiesProvider,
                                 IProtoStylingConventionsStrategiesProvider,
                                 IExtractionStrategiesProvider,
                                 INewTypeNamingStrategiesProvider

    {
        // Noting to do.
    }
}
