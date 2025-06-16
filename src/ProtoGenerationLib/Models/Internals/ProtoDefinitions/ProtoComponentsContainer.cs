using ProtoGenerationLib.Models.Internals.ProtoDefinitions;
using System.Collections.Generic;
using System.Linq;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.Utilities.CollectionUtilities;

namespace ProtoGenerationLib.Models.Internals.ProtoDefinitions
{
    /// <inheritdoc cref="IProtoComponentsContainer"/>
    public class ProtoComponentsContainer : IProtoComponentsContainer
    {
        /// <inheritdoc cref="IProtoComponentsContainer.Services"/>
        public List<IServiceDefinition> Services { get; set; }
        IEnumerable<IServiceDefinition> IProtoComponentsContainer.Services => Services;

        /// <inheritdoc cref="IProtoComponentsContainer.Messages"/>
        public List<IMessageDefinition> Messages { get; set; }
        IEnumerable<IMessageDefinition> IProtoComponentsContainer.Messages => Messages;

        /// <inheritdoc cref="IProtoComponentsContainer.Enums"/>
        public List<IEnumDefinition> Enums { get; set; }
        IEnumerable<IEnumDefinition> IProtoComponentsContainer.Enums => Enums;

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="ProtoComponentsContainer"/> class.
        /// </summary>
        public ProtoComponentsContainer()
        {
            Services = new List<IServiceDefinition>();
            Messages = new List<IMessageDefinition>();
            Enums = new List<IEnumDefinition>();
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoComponentsContainer"/> class.
        /// </summary>
        /// <param name="services"><inheritdoc cref="Services" path="/node()"/></param>
        /// <param name="messages"><inheritdoc cref="Messages" path="/node()"/></param>
        /// <param name="enums"><inheritdoc cref="Enums" path="/node()"/></param>
        public ProtoComponentsContainer(IEnumerable<IServiceDefinition> services,
                                        IEnumerable<IMessageDefinition> messages,
                                        IEnumerable<IEnumDefinition> enums)
        {
            Services = services.ToList();
            Messages = messages.ToList();
            Enums = enums.ToList();
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoComponentsContainer"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public ProtoComponentsContainer(IProtoComponentsContainer other)
        {
            Services = other.Services.Select(x => new ServiceDefinition(x)).Cast<IServiceDefinition>().ToList();
            Messages = other.Messages.Select(x => new MessageDefinition(x)).Cast<IMessageDefinition>().ToList();
            Enums = other.Enums.Select(x => new EnumDefinition(x)).Cast<IEnumDefinition>().ToList();
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as ProtoComponentsContainer;
            return other != null
                   && Services.SequenceEqual(other.Services)
                   && Messages.SequenceEqual(other.Messages)
                   && Enums.SequenceEqual(other.Enums);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Services.CalcHashCode(),
                    Messages.CalcHashCode(),
                    Enums.CalcHashCode()).GetHashCode();
        }

        #endregion Object Overrides
    }
}
