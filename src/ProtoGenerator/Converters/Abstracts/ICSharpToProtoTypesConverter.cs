using ProtoGenerator.Configurations.Abstracts;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using System.Collections.Generic;
using System;

namespace ProtoGenerator.Converters.Abstracts
{
    /// <summary>
    /// Converter between csharp types to proto types.
    /// </summary>
    public interface ICSharpToProtoTypesConverter
    {
        /// <summary>
        /// Convert the given csharp <paramref name="types"/> to proto components.
        /// </summary>
        /// <param name="types">The csharp types to convert.</param>
        /// <param name="conversionOptions">The options for the conversion.</param>
        /// <returns>A container that contains all the proto components that was create from the given <paramref name="types"/>.</returns>
        IProtoComponentsContainer Convert(IEnumerable<Type> types, IConversionOptions conversionOptions);
    }
}
