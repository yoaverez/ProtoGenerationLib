using ProtoGenerationLib.Attributes;
using System.Reflection;
using System;

namespace ProtoGenerationLib.Configurations.Delegates
{
    /// <summary>
    /// Encapsulates a method that gets a <paramref name="type"/> in the parameters
    /// and returns whether or not this <paramref name="type"/> is a proto service.
    /// </summary>
    /// <param name="type">The type to check.</param>
    /// <returns>
    /// This method should return <see langword="true"/> when the given <paramref name="type"/>
    /// is a proto service otherwise it should return <see langword="false"/>.
    /// </returns>
    public delegate bool IsProtoService(Type type);

    /// <summary>
    /// Encapsulates a method that gets a <paramref name="serviceType"/>, a <paramref name="method"/>
    /// and try to gets the <paramref name="method"/> <see cref="ProtoRpcType"/>.
    /// </summary>
    /// <param name="serviceType">The type of the service that declares the given <paramref name="method"/>.</param>
    /// <param name="method">The method to try getting the <see cref="ProtoRpcType"/> from.</param>
    /// <param name="rpcType">
    /// The <see cref="ProtoRpcType"/> of the given <paramref name="method"/>
    /// if the <paramref name="method"/> is rpc method otherwise any value is acceptable.
    /// </param>
    /// <returns>
    /// This method should return <see langword="true"/> when the given <paramref name="method"/>
    /// is rpc otherwise it should return <see langword="false"/>.
    /// </returns>
    public delegate bool TryGetRpcType(Type serviceType, MethodInfo method, out ProtoRpcType rpcType);
}
