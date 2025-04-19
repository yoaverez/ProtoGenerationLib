using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.Utilities.CollectionUtilities;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerator.Models.Internals.ProtoDefinitions
{
    /// <inheritdoc cref="IProtoDefinition"/>
    public class ProtoDefinition : IProtoDefinition
    {
        /// <inheritdoc/>
        public string Syntax { get; set; }

        /// <inheritdoc/>
        public string Package { get; set; }

        /// <inheritdoc cref="IProtoDefinition.Imports"/>
        public List<string> Imports { get; set; }
        IEnumerable<string> IProtoDefinition.Imports => Imports;

        /// <inheritdoc cref="IProtoDefinition.Services"/>
        public List<IServiceDefinition> Services {  get; set; }
        IEnumerable<IServiceDefinition> IProtoDefinition.Services => Services;

        /// <inheritdoc cref="IProtoDefinition.Messages"/>
        public List<IMessageDefinition> Messages { get; set; }
        IEnumerable<IMessageDefinition> IProtoDefinition.Messages => Messages;

        /// <inheritdoc cref="IProtoDefinition.Enums"/>
        public List<IEnumDefinition> Enums;
        IEnumerable<IEnumDefinition> IProtoDefinition.Enums => Enums;

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="ProtoDefinition"/> class.
        /// </summary>
        public ProtoDefinition()
        {
            Syntax = string.Empty;
            Package = string.Empty;
            Imports = new List<string>();
            Services = new List<IServiceDefinition>();
            Messages = new List<IMessageDefinition>();
            Enums = new List<IEnumDefinition>();
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoDefinition"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public ProtoDefinition(IProtoDefinition other)
        {
            Syntax = other.Syntax;
            Package = other.Package;
            Imports = other.Imports.ToList();
            Services = other.Services.Select(x => new ServiceDefinition(x)).Cast<IServiceDefinition>().ToList();
            Messages = other.Messages.Select(x => new MessageDefinition(x)).Cast<IMessageDefinition>().ToList();
            Enums = other.Enums.Select(x => new EnumDefinition(x)).Cast<IEnumDefinition>().ToList();
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as ProtoDefinition;
            return other != null
                   && Syntax.Equals(other.Syntax)
                   && Package.Equals(other.Package)
                   && Imports.SequenceEqual(other.Imports)
                   && Services.SequenceEqual(other.Services)
                   && Messages.SequenceEqual(other.Messages)
                   && Enums.SequenceEqual(other.Enums);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Syntax,
                    Package,
                    Imports.CalcHashCode(),
                    Services.CalcHashCode(),
                    Messages.CalcHashCode(),
                    Enums.CalcHashCode()).GetHashCode();
        }

        #endregion Object Overrides
    }
}
