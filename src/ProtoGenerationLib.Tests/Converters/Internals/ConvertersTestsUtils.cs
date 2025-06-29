using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using System.Reflection;

namespace ProtoGenerationLib.Tests.Converters.Internals
{
    internal static class ConvertersTestsUtils
    {
        /// <inheritdoc cref="EnumTypeMetadata(Type, List{IEnumValueMetadata})"/>
        public static EnumTypeMetadata CreateEnumTypeMetadata(Type type, List<IEnumValueMetadata> values)
        {
            return new EnumTypeMetadata(type, values);
        }

        /// <inheritdoc cref="EnumValueMetadata(string, int)"/>
        public static EnumValueMetadata CreateEnumValueMetadata(string name, int value)
        {
            return new EnumValueMetadata(name, value);
        }

        /// <inheritdoc cref="ContractTypeMetadata(Type, IEnumerable{IMethodMetadata})"/>
        public static ContractTypeMetadata CreateContractTypeMetadata(Type type, IEnumerable<IMethodMetadata> methods, string? documentation = null)
        {
            if (documentation is not null)
                return new ContractTypeMetadata(type, methods, documentation);
            return new ContractTypeMetadata(type, methods);
        }

        /// <inheritdoc cref="MethodMetadata(MethodInfo, Type, IEnumerable{IMethodParameterMetadata})"/>
        public static MethodMetadata CreateMethodMetadata(MethodInfo methodInfo, Type returnType, IEnumerable<IMethodParameterMetadata> parameters, string? documentation = null)
        {
            if (documentation is not null)
                return new MethodMetadata(methodInfo, returnType, parameters, documentation);
            return new MethodMetadata(methodInfo, returnType, parameters);
        }

        /// <inheritdoc cref="MethodParameterMetadata(Type, string)"/>
        public static MethodParameterMetadata CreateMethodParameterMetadata(Type type, string name)
        {
            return new MethodParameterMetadata(type, name);
        }

        /// <inheritdoc cref="DataTypeMetadata(Type, IEnumerable{IFieldMetadata}, IEnumerable{IDataTypeMetadata}, IEnumerable{IEnumTypeMetadata})"/>
        public static DataTypeMetadata CreateDataTypeMetadata(Type type, IEnumerable<IFieldMetadata> fields, IEnumerable<IDataTypeMetadata> nestedDataTypes, IEnumerable<IEnumTypeMetadata> nestedEnumTypes, string? documentation = null)
        {
            if (documentation is not null)
                return new DataTypeMetadata(type, fields, nestedDataTypes, nestedEnumTypes, documentation);
            return new DataTypeMetadata(type, fields, nestedDataTypes, nestedEnumTypes);
        }

        /// <inheritdoc cref="FieldMetadata(Type, string, IEnumerable{Attribute}, Type)"/>
        public static FieldMetadata CreateFieldMetadata(Type type, string name, Type declaringType, IEnumerable<Attribute>? attributes = null, string? documentation = null)
        {
            if (documentation is not null)
                return new FieldMetadata(type, name, attributes ?? new List<Attribute>(), declaringType, documentation);
            return new FieldMetadata(type, name, attributes ?? new List<Attribute>(), declaringType);
        }
    }
}
