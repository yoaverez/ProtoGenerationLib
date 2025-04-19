using System.Collections.Generic;

namespace ProtoGenerator.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// Represents a message in a Protocol Buffer definition, equivalent to a C# class.
    /// </summary>
    public interface IMessageDefinition : IProtoObject
    {
        /// <summary>
        /// The collection of fields in the message.
        /// </summary>
        IEnumerable<IFieldDefinition> Fields { get; }

        /// <summary>
        /// The collection of nested messages in the message.
        /// </summary>
        IEnumerable<IMessageDefinition> NestedMessages { get; }

        /// <summary>
        /// The collection of nested enums in the message.
        /// </summary>
        IEnumerable<IEnumDefinition> NestedEnums { get; }
    }
}
