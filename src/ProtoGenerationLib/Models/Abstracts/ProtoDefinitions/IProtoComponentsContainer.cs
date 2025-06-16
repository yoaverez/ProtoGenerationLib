using System.Collections.Generic;

namespace ProtoGenerationLib.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// A container for all possible proto types.
    /// </summary>
    public interface IProtoComponentsContainer
    {
        /// <summary>
        /// The contained services.
        /// </summary>
        IEnumerable<IServiceDefinition> Services { get; }

        /// <summary>
        /// The contained messages.
        /// </summary>
        IEnumerable<IMessageDefinition> Messages { get; }

        /// <summary>
        /// The contained enums.
        /// </summary>
        IEnumerable<IEnumDefinition> Enums { get; }
    }
}
