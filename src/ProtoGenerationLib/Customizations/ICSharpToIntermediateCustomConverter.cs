using System;

namespace ProtoGenerationLib.Customizations
{
    /// <summary>
    /// Contract for user custom converter.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate representation object.</typeparam>
    public interface ICSharpToIntermediateCustomConverter<TIntermediate> : ICustomTypesExtractor
    {
        /// <summary>
        /// Convert the given <paramref name="type"/> to it's intermediate representation.
        /// </summary>
        /// <param name="type">The type to convert from csharp to intermediate representation.</param>
        /// <returns>The intermediate representation of the given <paramref name="type"/>.</returns>
        TIntermediate ConvertTypeToIntermediateRepresentation(Type type);
    }
}
