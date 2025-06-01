using System.Collections.Generic;

namespace ProtoGenerator.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// Represents a the common ground of proto objects.
    /// </summary>
    public interface IProtoObject
    {
        /// <summary>
        /// The name of the proto object.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// The package of the proto object.
        /// </summary>
        string Package { get; }

        /// <summary>
        /// The imports that are needed for the proto object to work.
        /// </summary>
        ISet<string> Imports { get; }
    }
}
