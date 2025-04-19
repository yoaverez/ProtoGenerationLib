using System.Collections.Generic;

namespace ProtoGenerator.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// Represents a service in a Protocol Buffer language.
    /// </summary>
    public interface IServiceDefinition : IProtoObject
    {
        /// <summary>
        /// The service RPC definitions.
        /// </summary>
        IEnumerable<IRpcDefinition> RpcMethods { get; }
    }
}
