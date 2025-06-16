using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Discovery.Abstracts
{
    /// <summary>
    /// A discoverer of proto type metadata.
    /// </summary>
    public interface IProtoTypeMetadataDiscoverer
    {
        /// <summary>
        /// Discover the given <paramref name="types"/> matching proto type metadata.
        /// </summary>
        /// <param name="types">The types from which to discover the proto metadata.</param>
        /// <param name="protoGeneratorConfiguration">The options for the discovery.</param>
        /// <returns>A mapping between a type to its matching proto type metadata.</returns>
        Dictionary<Type, IProtoTypeMetadata> DiscoverProtosMetadata(IEnumerable<Type> types,
                                                                    IProtoGenerationOptions protoGeneratorConfiguration);
    }
}
