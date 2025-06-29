using ProtoGenerationLib.Models.Internals.IntermediateRepresentations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtoGenerationLib.Tests.Extractors.Internals.TypesExtractors
{
    internal static class TypesExtractorsUtils
    {
        /// <summary>
        /// Create a new instance of <see cref="FieldMetadata"/>.
        /// </summary>
        /// <param name="type">The type of the field.</param>
        /// <param name="name">The name of the field.</param>
        /// <param name="declaringType">The type that has this field.</param>
        /// <param name="attributes">The attributes of the field.</param>
        /// <returns>A new instance of <see cref="FieldMetadata"/>.</returns>
        public static FieldMetadata CreateFieldMetadata(Type type, string name, Type declaringType, IEnumerable<Attribute>? attributes = null, string documentation = "")
        {
            attributes = attributes ?? new List<Attribute>();
            return new FieldMetadata(type, name, attributes, declaringType, documentation);
        }
    }
}
