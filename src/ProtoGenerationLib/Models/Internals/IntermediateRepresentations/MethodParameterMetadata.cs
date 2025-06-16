using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using System;
using System.Reflection;

namespace ProtoGenerationLib.Models.Internals.IntermediateRepresentations
{
    /// <inheritdoc cref="IMethodParameterMetadata"/>
    public class MethodParameterMetadata : IMethodParameterMetadata
    {
        /// <inheritdoc/>
        public Type Type { get; set; }

        /// <inheritdoc/>
        public string Name { get; set; }

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="MethodParameterMetadata"/> class.
        /// </summary>
        public MethodParameterMetadata()
        {
            // Noting to do.
        }

        /// <summary>
        /// Create new instance of the <see cref="MethodParameterMetadata"/> class.
        /// </summary>
        /// <param name="type"><inheritdoc cref="Type" path="/node()"/></param>
        /// <param name="name"><inheritdoc cref="Name" path="/node()"/></param>
        public MethodParameterMetadata(Type type, string name)
        {
            Type = type;
            Name = name;
        }

        /// <summary>
        /// Create new instance of the <see cref="MethodParameterMetadata"/> class.
        /// </summary>
        /// <param name="parameterInfo">The parameter info from which to create the <see cref="MethodParameterMetadata"/>.</param>
        public MethodParameterMetadata(ParameterInfo parameterInfo)
        {
            Type = parameterInfo.ParameterType;
            Name = parameterInfo.Name;
        }

        /// <summary>
        /// Create new instance of the <see cref="MethodParameterMetadata"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public MethodParameterMetadata(IMethodParameterMetadata other)
        {
            Type = other.Type;
            Name = other.Name;
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as MethodParameterMetadata;
            return other != null
                   && Type.Equals(other.Type)
                   && Name.Equals(other.Name);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Type,
                    Name).GetHashCode();
        }

        #endregion Object Overrides
    }
}
