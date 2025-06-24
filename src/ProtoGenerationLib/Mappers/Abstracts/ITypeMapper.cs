using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using System;

namespace ProtoGenerationLib.Mappers.Abstracts
{
    /// <summary>
    /// Mapper between csharp type to its <see cref="IProtoTypeMetadata"/>.
    /// </summary>
    public interface ITypeMapper
    {
        /// <summary>
        /// Check whether or not the given <paramref name="type"/> can be handled
        /// by the this mapper.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/>
        /// can be handled by this mapper otherwise <see langword="false"/>.
        /// </returns>
        bool CanHandle(Type type);

        /// <summary>
        /// Map the given <paramref name="type"/> to its <see cref="IProtoTypeMetadata"/>.
        /// </summary>
        /// <param name="type">The type to map.</param>
        /// <returns>The <see cref="IProtoTypeMetadata"/> that represents the given <paramref name="type"/>.</returns>
        IProtoTypeMetadata MapTypeToProtoMetadata(Type type);
    }
}
