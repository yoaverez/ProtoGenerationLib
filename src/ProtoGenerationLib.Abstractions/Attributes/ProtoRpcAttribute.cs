using System;

namespace ProtoGenerationLib.Attributes
{
    /// <summary>
    /// Attribute for marking methods as a proto service methods.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ProtoRpcAttribute : Attribute
    {
        /// <summary>
        /// Represents the type of the RPC.
        /// </summary>
        public ProtoRpcType RpcType { get; }

        /// <summary>
        /// Create new instances of the <see cref="ProtoRpcAttribute"/> class.
        /// </summary>
        /// <param name="rpcType">The RPC method type.</param>
        public ProtoRpcAttribute(ProtoRpcType rpcType)
        {
            RpcType = rpcType;
        }
    }
}
