using System;

namespace ProtoGenerator.Strategies.Abstracts
{
    /// <summary>
    /// A naming strategy for type naming.
    /// </summary>
    public interface ITypeNamingStrategy
    {
        /// <summary>
        /// Get the protobuf message name of the given <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type whose protobuf message name you want.</param>
        /// <returns>The protobuf message name of the given <paramref name="type"/>.</returns>
        string GetTypeName(Type type);
    }
}
