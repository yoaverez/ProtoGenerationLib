using System.Collections.Generic;

namespace ProtoGenerator.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// Represents a complete Protocol Buffer file definition with all its components.
    /// </summary>
    public interface IProtoDefinition
    {
        /// <summary>
        /// The Protocol Buffer syntax version (e.g., "proto3").
        /// </summary>
        string Syntax { get; }

        /// <summary>
        /// The package name for the Protocol Buffer definition.
        /// </summary>
        string Package { get; }

        /// <summary>
        /// The collection of import statements for the Protocol Buffer definition.
        /// </summary>
        ISet<string> Imports { get; }

        /// <summary>
        /// The collection of service definitions in the Protocol Buffer definition.
        /// </summary>
        IEnumerable<IServiceDefinition> Services { get; }

        /// <summary>
        /// The collection of message definitions in the Protocol Buffer definition.
        /// </summary>
        IEnumerable<IMessageDefinition> Messages { get; }

        /// <summary>
        /// The collection of enum definitions in the Protocol Buffer definition.
        /// </summary>
        IEnumerable<IEnumDefinition> Enums { get; }
    }
}
