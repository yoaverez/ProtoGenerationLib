using ProtoGenerator.Extractors.Abstracts;

namespace ProtoGenerator.Converters.Abstracts
{
    /// <summary>
    /// Contract for user custom converter.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate representation object.</typeparam>
    public interface ICSharpToIntermediateCustomConverter<TIntermediate> : ICSharpToIntermediateConverter<TIntermediate>, ITypesExtractor
    {
        // Noting to do.
    }
}
