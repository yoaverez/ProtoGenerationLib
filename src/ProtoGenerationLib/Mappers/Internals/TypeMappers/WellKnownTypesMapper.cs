using ProtoGenerationLib.Constants;
using ProtoGenerationLib.Mappers.Abstracts;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using System;
using System.Collections.Generic;

namespace ProtoGenerationLib.Mappers.Internals.TypeMappers
{
    /// <summary>
    /// A mapper from well known csharp types to their well known proto metadatas.
    /// </summary>
    internal class WellKnownTypesMapper : ITypeMapper
    {
        /// <summary>
        /// Mapping from csharp type to its well known proto type metadata.
        /// </summary>
        private IReadOnlyDictionary<Type, IProtoTypeMetadata> wellKnownTypesProtoMetadatas;

        /// <summary>
        /// Create new instance of the <see cref="WellKnownTypesMapper"/> class.
        /// </summary>
        /// <param name="wellKnownTypesProtoMetadatas"></param>
        public WellKnownTypesMapper(IReadOnlyDictionary<Type, IProtoTypeMetadata>? wellKnownTypesProtoMetadatas = null)
        {
            this.wellKnownTypesProtoMetadatas = wellKnownTypesProtoMetadatas ?? WellKnownTypesConstants.WellKnownTypes;
        }

        /// <inheritdoc/>
        public bool CanHandle(Type type)
        {
            return wellKnownTypesProtoMetadatas.ContainsKey(type);
        }

        /// <inheritdoc/>
        public IProtoTypeMetadata MapTypeToProtoMetadata(Type type)
        {
            if (!CanHandle(type))
                throw new ArgumentException();

            return wellKnownTypesProtoMetadatas[type];
        }
    }
}
