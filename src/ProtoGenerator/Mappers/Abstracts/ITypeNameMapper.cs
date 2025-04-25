using System;

namespace ProtoGenerator.Mappers.Abstracts
{
    /// <summary>
    /// Mapper between csharp type to it proto type name.
    /// </summary>
    public interface ITypeNameMapper
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
        /// Map the given <paramref name="type"/> to it's proto type name.
        /// </summary>
        /// <param name="type">The type to map.</param>
        /// <returns>The name of the proto type that will represent the given <paramref name="type"/>.</returns>
        string MapType(Type type);
    }
}
