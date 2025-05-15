using System;

namespace ProtoGenerator.Strategies.Abstracts
{
    /// <summary>
    /// A strategy for choosing a name to new type this is needed for a proto.
    /// </summary>
    public interface INewTypeNamingStrategy
    {
        /// <summary>
        /// Gets the name of the new proto type that is based on the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The origin type of the new type.</param>
        /// <returns>The name of the new proto type that is based on the given <paramref name="type"/>.</returns>
        string GetNewTypeName(Type type);
    }
}
