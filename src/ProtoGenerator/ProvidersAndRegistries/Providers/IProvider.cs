namespace ProtoGenerator.ProvidersAndRegistries.Providers
{
    /// <summary>
    /// Provider that provide all the customizations.
    /// </summary>
    public interface IProvider : ICustomConvertersProvider,
                                 ICustomTypeNameMappersProvider,
                                 INumberingStrategiesProvider,
                                 IProtoNamingStrategiesProvider,
                                 IProtoStylingConventionsStrategiesProvider,
                                 IExtractionStrategiesProvider

    {
        // Noting to do.
    }
}
