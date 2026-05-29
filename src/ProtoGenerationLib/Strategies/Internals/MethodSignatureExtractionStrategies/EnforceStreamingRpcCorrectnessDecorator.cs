using ProtoGenerationLib.Abstractions;
using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Abstractions.Streams;
using ProtoGenerationLib.CommonUtilities;
using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using ProtoGenerationLib.Strategies.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ProtoGenerationLib.Strategies.Internals.MethodSignatureExtractionStrategies
{
    /// <summary>
    /// Decorator for a given method signature extraction strategy.
    /// This decorator enforces the correctness of the different rpc methods (unary and all type of streaming)
    /// i.e. it makes sure that unary rpc does not have any stream like parameters and return type
    /// and for streaming rpcs it does the opposite checks depending on the streaming type (client, server or bidirectional).
    /// </summary>
    /// <remarks>
    /// <b>This decorator throws <see cref="ArgumentException"/> if the method does not have appropriate signature for its
    /// rpc type.</b>
    /// </remarks>
    public class EnforceStreamingRpcCorrectnessDecorator : MethodSignatureExtractionStrategyDecorator
    {
        /// <summary>
        /// The basic types that all channels or call responses should descendants of.
        /// </summary>
        private static readonly IEnumerable<Type> allChannelsTypes;

        /// <summary>
        /// Initialize the <see cref="EnforceStreamingRpcCorrectnessDecorator"/> class
        /// static members.
        /// </summary>
        static EnforceStreamingRpcCorrectnessDecorator()
        {
            allChannelsTypes = new Type[]
            {
                typeof(IStreamReader<>),
                typeof(IStreamWriter<>),
                typeof(IClientStreamingResponse<,>),
                typeof(IDuplexStreamingResponse<,>),
            };
        }

        /// <summary>
        /// Create new instance of the <see cref="EnforceStreamingRpcCorrectnessDecorator"/> class.
        /// </summary>
        /// <param name="underlineStrategy"><inheritdoc cref="MethodSignatureExtractionStrategyDecorator(IMethodSignatureExtractionStrategy)" path="/param[@name='underlineStrategy']"/></param>
        public EnforceStreamingRpcCorrectnessDecorator(IMethodSignatureExtractionStrategy underlineStrategy) : base(underlineStrategy)
        {
            // Noting to do.
        }

        /// <inheritdoc/>
        protected override (Type ReturnType, IEnumerable<IMethodParameterMetadata> Parameters) DecorateStrategy(MethodInfo method, Type returnType, IEnumerable<IMethodParameterMetadata> parameters, IAnalysisOptions analysisOptions)
        {
            var methodRpcType = method.GetMethodRpcType(method.DeclaringType, analysisOptions);
            return methodRpcType switch
            {
                ProtoRpcType.Unary => HandleUnaryCall(method, returnType, parameters),
                ProtoRpcType.ServerStreaming => HandleServerStreamingCall(method, returnType, parameters),
                ProtoRpcType.ClientStreaming => HandleClientStreamingCall(method, returnType, parameters),
                ProtoRpcType.BidirectionalStreaming => HandleBidirectionalStreamingCall(method, returnType, parameters),
                _ => throw new NotSupportedException($"" +
                                                     $"{GetExceptionPrefix(method)}" +
                                                     $" method rpc type: {methodRpcType}, is not supported."),
            };
        }

        /// <summary>
        /// Make sure that the given <paramref name="method"/> signature is a valid unary call.
        /// i.e. It does not contains any channel types in its signature.
        /// </summary>
        /// <param name="method">The c# method that represents the unary rpc.</param>
        /// <param name="returnType">The underline strategy return type result.</param>
        /// <param name="parameters">The underline strategy parameters result.</param>
        /// <returns>The unmodified result of the underline strategy.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown if the given <paramref name="returnType"/> or <b>any</b> of the given <paramref name="parameters"/>
        /// are a descendants of any of the supported channel types.
        /// </exception>
        private (Type ReturnType, IEnumerable<IMethodParameterMetadata> Parameters) HandleUnaryCall(MethodInfo method, Type returnType, IEnumerable<IMethodParameterMetadata> parameters)
        {
            ThrowIfReturnTypeIsChannel(method, returnType, ProtoRpcType.Unary);
            ThrowIfAnyParameterIsChannel(method, parameters, ProtoRpcType.Unary);

            // There is noting to modify.
            return (returnType, parameters);
        }

        /// <summary>
        /// Make sure that the given <paramref name="method"/> signature is a valid server streaming call.
        /// i.e. It does not contains any channel types in its parameters and its return type
        /// is a descendant of <see cref="IStreamReader{T}"/>.
        /// </summary>
        /// <param name="method">The c# method that represents the server streaming rpc.</param>
        /// <param name="returnType">The underline strategy return type result.</param>
        /// <param name="parameters">The underline strategy parameters result.</param>
        /// <returns>
        /// Modify the given <paramref name="returnType"/> to be the stream reader data type.
        /// The <paramref name="parameters"/> remains the same.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if:
        /// <list type="bullet">
        /// <item>The given <paramref name="returnType"/> is not a descendant of <see cref="IStreamReader{T}"/>.</item>
        /// <item><b>Any</b> of the given <paramref name="parameters"/> are a descendants of any of the supported channel types.</item>
        /// </list>
        /// </exception>
        private (Type ReturnType, IEnumerable<IMethodParameterMetadata> Parameters) HandleServerStreamingCall(MethodInfo method, Type returnType, IEnumerable<IMethodParameterMetadata> parameters)
        {
            ThrowIfActualTypeIsNotADescendant(method, returnType, typeof(IStreamReader<>), ProtoRpcType.ServerStreaming, out var closestAncestor);

            ThrowIfAnyParameterIsChannel(method, parameters, ProtoRpcType.ServerStreaming);

            var dataType = closestAncestor!.GetGenericArguments()[0];

            // Modify the return type to be the data type of the channel.
            return (dataType, parameters);
        }

        /// <summary>
        /// Make sure that the given <paramref name="method"/> signature is a valid client streaming call.
        /// i.e. It does have any parameters and its return type is either a pair (tuple or key value pair)
        /// in which the first generic argument is a descendant of <see cref="IStreamWriter{T}"/> and the second
        /// generic argument is a descendant of <see cref="Task{TResult}"/>
        /// <b>or</b> the return type is a descendant of <see cref="IClientStreamingResponse{TRequest, TResponse}"/>.
        /// </summary>
        /// <param name="method">The c# method that represents the client streaming rpc.</param>
        /// <param name="returnType">The underline strategy return type result.</param>
        /// <param name="parameters">The underline strategy parameters result.</param>
        /// <returns>
        /// Modify the given <paramref name="returnType"/> to be the either the element type of the second generic argument <see cref="Task{TResult}"/>
        /// if <paramref name="returnType"/> is a pair, of the <paramref name="returnType"/> second generic parameter.
        /// The <paramref name="parameters"/> will contains a single object which represents the request stream type
        /// which is the element type of the pair first generic argument or the first generic argument of the <paramref name="returnType"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if:
        /// <list type="bullet">
        /// <item>Given <paramref name="parameters"/> is not empty.</item>
        /// <item>The given <paramref name="returnType"/> is not a valid pair or a descendant of <see cref="IClientStreamingResponse{TRequest, TResponse}"/>.</item>
        /// </list>
        /// </exception>
        private (Type ReturnType, IEnumerable<IMethodParameterMetadata> Parameters) HandleClientStreamingCall(MethodInfo method, Type returnType, IEnumerable<IMethodParameterMetadata> parameters)
        {
            return HandleClientOrBidirectionalStreamingCall(method,
                                                            returnType,
                                                            parameters,
                                                            typeof(IClientStreamingResponse<,>),
                                                            typeof(Task<>),
                                                            ProtoRpcType.ClientStreaming);
        }

        /// <summary>
        /// Make sure that the given <paramref name="method"/> signature is a valid bidirectional streaming call.
        /// i.e. It does have any parameters and its return type is either a pair (tuple or key value pair)
        /// in which the first generic argument is a descendant of <see cref="IStreamWriter{T}"/> and the second
        /// generic argument is a descendant of <see cref="IStreamReader{T}"/>
        /// <b>or</b> the return type is a descendant of <see cref="IDuplexStreamingResponse{TRequest, TResponse}"/>.
        /// </summary>
        /// <param name="method">The c# method that represents the bidirectional streaming rpc.</param>
        /// <param name="returnType">The underline strategy return type result.</param>
        /// <param name="parameters">The underline strategy parameters result.</param>
        /// <returns>
        /// Modify the given <paramref name="returnType"/> to be the either the element type of the second generic argument <see cref="IStreamReader{T}"/>
        /// if <paramref name="returnType"/> is a pair, of the <paramref name="returnType"/> second generic parameter.
        /// The <paramref name="parameters"/> will contains a single object which represents the request stream type
        /// which is the element type of the pair first generic argument or the first generic argument of the <paramref name="returnType"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if:
        /// <list type="bullet">
        /// <item>Given <paramref name="parameters"/> is not empty.</item>
        /// <item>The given <paramref name="returnType"/> is not a valid pair or a descendant of <see cref="IDuplexStreamingResponse{TRequest, TResponse}"/>.</item>
        /// </list>
        /// </exception>
        private (Type ReturnType, IEnumerable<IMethodParameterMetadata> Parameters) HandleBidirectionalStreamingCall(MethodInfo method, Type returnType, IEnumerable<IMethodParameterMetadata> parameters)
        {
            return HandleClientOrBidirectionalStreamingCall(method,
                                                            returnType,
                                                            parameters,
                                                            typeof(IDuplexStreamingResponse<,>),
                                                            typeof(IStreamReader<>),
                                                            ProtoRpcType.BidirectionalStreaming);
        }

        /// <summary>
        /// Auxiliary method to handle both client streaming call and bidirectional streaming call.
        /// </summary>
        /// <param name="method">The c# method that represents the client or bidirectional streaming rpc.</param>
        /// <param name="returnType">The underline strategy return type result.</param>
        /// <param name="parameters">The underline strategy parameters result.</param>
        /// <param name="fullResponseRoot">The root type that contains the rpc 2 results.</param>
        /// <param name="pairResponseRoot">The root type that represents only the rpc response type.</param>
        /// <param name="rpcType">The type of the rpc.</param>
        /// <returns>
        /// If <paramref name="rpcType"/> is <see cref="ProtoRpcType.ClientStreaming"/>:
        /// <para>
        /// <inheritdoc cref="HandleClientStreamingCall(MethodInfo, Type, IEnumerable{IMethodParameterMetadata})" path="/returns"/>
        /// </para>
        /// Otherwise <paramref name="rpcType"/> should be <see cref="ProtoRpcType.BidirectionalStreaming"/> and return:
        /// <para>
        /// <inheritdoc cref="HandleBidirectionalStreamingCall(MethodInfo, Type, IEnumerable{IMethodParameterMetadata})" path="/returns"/>
        /// </para>
        /// </returns>
        private (Type ReturnType, IEnumerable<IMethodParameterMetadata> Parameters) HandleClientOrBidirectionalStreamingCall(MethodInfo method,
                                                                                                                             Type returnType,
                                                                                                                             IEnumerable<IMethodParameterMetadata> parameters,
                                                                                                                             Type fullResponseRoot,
                                                                                                                             Type pairResponseRoot,
                                                                                                                             ProtoRpcType rpcType)
        {
            ThrowIfThereAreParameters(method, parameters, rpcType);

            Type? requestType;
            Type? responseType;

            // If the return type is a tuple or a key value pair
            // check that one type is a descendant of a write stream
            // and the other is a Task of the response data.
            if (TryGetPairTypes(returnType, out var requestWrapperType, out var responseWrapperType))
            {
                ThrowIfActualTypeIsNotADescendant(method, requestWrapperType!, typeof(IStreamWriter<>), rpcType, out var closestAncestor);
                requestType = closestAncestor!.GetGenericArguments()[0];

                ThrowIfActualTypeIsNotADescendant(method, responseWrapperType!, pairResponseRoot, rpcType, out var closestAncestor2);
                responseType = closestAncestor2!.GetGenericArguments()[0];
            }
            else
            {
                ThrowIfActualTypeIsNotADescendant(method, returnType, fullResponseRoot, rpcType, out var closestAncestor);

                var fullResponseGenerics = closestAncestor!.GetGenericArguments();
                requestType = fullResponseGenerics[0];
                responseType = fullResponseGenerics[1];
            }

            var newParameters = new List<IMethodParameterMetadata> { new MethodParameterMetadata(requestType, "requestStream") };
            return (responseType!, newParameters);
        }

        /// <summary>
        /// Throw an <see cref="ArgumentException"/> if the given <paramref name="returnType"/> is
        /// a descendant of a channel type.
        /// </summary>
        /// <param name="method">The method whose signature has the given <paramref name="returnType"/>.</param>
        /// <param name="returnType">The type to check that it is not a descendant of a channel type.</param>
        /// <param name="protoRpcType">The type rpc that the <paramref name="method"/> represents.</param>
        private void ThrowIfReturnTypeIsChannel(MethodInfo method, Type returnType, ProtoRpcType protoRpcType)
        {
            if (allChannelsTypes.Any(channelType => returnType.TryGetClosestAncestorTo(channelType, out _)))
                throw new ArgumentException($"{GetExceptionPrefix(method)}{protoRpcType} rpc, return type can not be a descendant of a channel type. Rpc return type: {returnType.FullName}.");
        }

        /// <summary>
        /// Throw an <see cref="ArgumentException"/> if <b>any</b> of the given <paramref name="parameters"/> is
        /// a descendant of a channel type.
        /// </summary>
        /// <param name="method">The method whose signature has the given <paramref name="parameters"/>.</param>
        /// <param name="parameters">The parameters to check that all of them are not descendants of a channel type.</param>
        /// <param name="protoRpcType">The type rpc that the <paramref name="method"/> represents.</param>
        private void ThrowIfAnyParameterIsChannel(MethodInfo method, IEnumerable<IMethodParameterMetadata> parameters, ProtoRpcType protoRpcType)
        {
            var firstInvalidParameter = parameters.FirstOrDefault(parameter => allChannelsTypes.Any(channelType => parameter.Type.TryGetClosestAncestorTo(channelType, out _)));
            if (firstInvalidParameter is not null)
                throw new ArgumentException($"{GetExceptionPrefix(method)}{protoRpcType} rpc, parameter types can not be a descendant of a channel type. Rpc parameter: {firstInvalidParameter.Name}, {firstInvalidParameter.Type}.");
        }

        /// <summary>
        /// Throw an <see cref="ArgumentException"/> if the given <paramref name="parameters"/> is
        /// not empty.
        /// </summary>
        /// <param name="method">The method whose signature has the given <paramref name="parameters"/>.</param>
        /// <param name="parameters">The parameters to check.</param>
        /// <param name="protoRpcType">The type rpc that the <paramref name="method"/> represents.</param>
        private void ThrowIfThereAreParameters(MethodInfo method, IEnumerable<IMethodParameterMetadata> parameters, ProtoRpcType protoRpcType)
        {
            if (parameters.Any())
                throw new ArgumentException($"{GetExceptionPrefix(method)}{protoRpcType} rpc, must not have parameters. " +
                                            $"Given method has {parameters.Count()} parameters.");
        }

        /// <summary>
        /// Throw an <see cref="ArgumentException"/> if the given <paramref name="typeToCheck"/> is
        /// not a descendant of the given <paramref name="root"/>.
        /// </summary>
        /// <param name="method">The method whose signature is in check.</param>
        /// <param name="typeToCheck">The type to check if it is a descendant of <paramref name="root"/>.</param>
        /// <param name="root">The type to check if it is an ancestor of the given <paramref name="typeToCheck"/>.</param>
        /// <param name="rpcType">The type rpc that the <paramref name="method"/> represents.</param>
        /// <param name="closestAncestor">
        /// The closest ancestor of <paramref name="typeToCheck"/> to <paramref name="root"/>
        /// if <paramref name="typeToCheck"/> is a descendant of <paramref name="root"/> otherwise <see langword="null"/>.
        /// </param>
        private void ThrowIfActualTypeIsNotADescendant(MethodInfo method, Type typeToCheck, Type root, ProtoRpcType rpcType, out Type? closestAncestor)
        {
            if (!typeToCheck.TryGetClosestAncestorTo(root, out closestAncestor))
                throw new ArgumentException($"" +
                    $"{GetExceptionPrefix(method)}" +
                    $"{rpcType} rpc signature mismatch. " +
                    $"Given type: {typeToCheck.FullName}, must be a descendant of {root.FullName}.");
        }

        /// <summary>
        /// Get an exception prefix for all the exceptions that are thrown in this file.
        /// </summary>
        /// <param name="method">The method whose signature is in check.</param>
        /// <returns>A prefix that state the origin of all the exceptions that are thrown from this class.</returns>
        private string GetExceptionPrefix(MethodInfo method)
        {
            return $"{method.DeclaringType.Name}.{method.Name}: ";
        }

        /// <summary>
        /// Try getting a pair type (key value pair or tuple with 2 items) generic types.
        /// </summary>
        /// <param name="type">The supposed pair type.</param>
        /// <param name="firstType">The first generic type if <paramref name="type"/> is a pair type otherwise null.</param>
        /// <param name="secondType">The second generic type if <paramref name="type"/> is a pair type otherwise null.</param>
        /// <returns>
        /// <see langword="true"/> if the given <paramref name="type"/> is a pair type (key value pair or tuple with 2 items)
        /// otherwise <see langword="false"/>.
        /// </returns>
        private bool TryGetPairTypes(Type type, out Type? firstType, out Type? secondType)
        {
            firstType = null;
            secondType = null;

            if (type.IsGenericType && (type.IsTuple() || type.IsValueTuple() || type.GetGenericTypeDefinition().Equals(typeof(KeyValuePair<,>))))
            {
                var genericArguments = type.GetGenericArguments();

                if (genericArguments.Length == 2)
                {
                    firstType = genericArguments[0];
                    secondType = genericArguments[1];
                    return true;
                }
            }

            return false;
        }
    }
}
