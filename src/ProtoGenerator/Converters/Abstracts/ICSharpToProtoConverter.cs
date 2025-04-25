using ProtoGenerator.Configurations.Abstracts;
using System;

namespace ProtoGenerator.Converters.Abstracts
{
    /// <summary>
    /// Converter between csharp type to the type's proto representation.
    /// </summary>
    /// <typeparam name="TProtoDefinition">The type of the proto representation object.</typeparam>
    public interface ICSharpToProtoConverter<TProtoDefinition>
    {
        /// <summary>
        /// Convert the given <paramref name="type"/> to it's proto definition.
        /// </summary>
        /// <param name="type">The type to convert</param>
        /// <param name="conversionOptions">The options for the conversion.</param>
        /// <returns>The proto definition that represents the given <paramref name="type"/>.</returns>
        TProtoDefinition ConvertTypeToProtoDefinition(Type type, IConversionOptions conversionOptions);
    }
}
