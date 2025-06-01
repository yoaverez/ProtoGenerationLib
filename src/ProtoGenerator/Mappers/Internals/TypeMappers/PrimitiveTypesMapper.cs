using ProtoGenerator.Mappers.Abstracts;
using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using System;

namespace ProtoGenerator.Mappers.Internals.TypeMappers
{
    public class PrimitiveTypesMapper : ITypeNameMapper
    {
        public bool CanHandle(Type type)
        {
            return type.IsPrimitive;
        }

        public IProtoTypeBaseMetadata MapTypeToProtoMetaData(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
