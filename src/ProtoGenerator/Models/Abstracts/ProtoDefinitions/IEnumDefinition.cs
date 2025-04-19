using System.Collections.Generic;

namespace ProtoGenerator.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// Represents an enum in a Protocol Buffer definition, equivalent to a C# enum.
    /// </summary>
    public interface IEnumDefinition : IProtoObject
    {
        /// <summary>
        /// The collection of values in the enum.
        /// </summary>
        IEnumerable<IEnumValueDefinition> Values { get; }
    }
}
