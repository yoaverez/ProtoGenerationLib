using System;
using System.Collections.Generic;

namespace ProtoGenerator.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// Meta data of a proto type.
    /// </summary>
    public interface IProtoTypeMetadata : IProtoTypeBaseMetadata
    {
        /// <summary>
        /// The full name of the proto type.
        /// This is a combination of package, parent types names (heritage)
        /// and the name of the type.
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Flag that indicates whether or not this proto type is nested withing another proto type.
        /// </summary>
        bool IsNested { get; }

        /// <summary>
        /// A set containing all the direct descendants of this proto type.
        /// </summary>
        ISet<Type> NestedTypes { get; }
    }
}
