using ProtoGenerator.Constants;
using ProtoGenerator.Mappers.Abstracts;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using System;
using System.Collections.Generic;

namespace ProtoGenerator.Mappers.Internals.TypeMappers
{
    /// <summary>
    /// A mapper from well known csharp types to their well known proto metadatas.
    /// </summary>
    public class WellKnownTypesMapper : ITypeNameMapper
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
        public IProtoTypeBaseMetadata MapTypeToProtoMetaData(Type type)
        {
            if (!CanHandle(type))
                throw new ArgumentException();

            return wellKnownTypesProtoMetadatas[type];
        }
    }
}
