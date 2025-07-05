using ProtoGenerationLib.Models.Internals.ProtoDefinitions;
using System.Collections.Generic;
using System.Linq;
using ProtoGenerationLib.Models.Abstracts.ProtoDefinitions;
using ProtoGenerationLib.Utilities.CollectionUtilities;

namespace ProtoGenerationLib.Models.Internals.ProtoDefinitions
{
    /// <inheritdoc cref="IMessageDefinition"/>
    public class MessageDefinition : ProtoObject, IMessageDefinition
    {
        /// <inheritdoc cref="IMessageDefinition.Fields"/>
        public List<IFieldDefinition> Fields { get; set; }
        IEnumerable<IFieldDefinition> IMessageDefinition.Fields => Fields.AsReadOnly();

        /// <inheritdoc cref="IMessageDefinition.NestedMessages"/>
        public List<IMessageDefinition> NestedMessages { get; set; }
        IEnumerable<IMessageDefinition> IMessageDefinition.NestedMessages => NestedMessages.AsReadOnly();

        /// <inheritdoc cref="IMessageDefinition.NestedEnums"/>
        public List<IEnumDefinition> NestedEnums { get; set; }
        IEnumerable<IEnumDefinition> IMessageDefinition.NestedEnums => NestedEnums.AsReadOnly();

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="MessageDefinition"/> class.
        /// </summary>
        public MessageDefinition() : base()
        {
            Fields = new List<IFieldDefinition>();
            NestedMessages = new List<IMessageDefinition>();
            NestedEnums = new List<IEnumDefinition>();
        }

        /// <summary>
        /// Create new instance of the <see cref="MessageDefinition"/> class.
        /// </summary>
        /// <param name="name"><inheritdoc cref="ProtoObject.Name" path="/node()"/></param>
        /// <param name="package"><inheritdoc cref="ProtoObject.Package" path="/node()"/></param>
        /// <param name="imports"><inheritdoc cref="ProtoObject.Imports" path="/node()"/></param>
        /// <param name="fields"><inheritdoc cref="Fields" path="/node()"/></param>
        /// <param name="nestedMessages"><inheritdoc cref="NestedMessages" path="/node()"/></param>
        /// <param name="nestedEnums"><inheritdoc cref="NestedEnums" path="/node()"/></param>
        public MessageDefinition(string name,
                                 string package,
                                 IEnumerable<string> imports,
                                 IEnumerable<IFieldDefinition> fields,
                                 IEnumerable<IMessageDefinition> nestedMessages,
                                 IEnumerable<IEnumDefinition> nestedEnums) : base(name, package, imports)
        {
            Fields = fields.ToList();
            NestedMessages = nestedMessages.ToList();
            NestedEnums = nestedEnums.ToList();
        }

        /// <inheritdoc cref="MessageDefinition(string, string, IEnumerable{string}, IEnumerable{IFieldDefinition}, IEnumerable{IMessageDefinition}, IEnumerable{IEnumDefinition})"/>
        /// <inheritdoc cref="ProtoObject(string, string, IEnumerable{string}, string)" path="/param"/>
        public MessageDefinition(string name,
                                 string package,
                                 IEnumerable<string> imports,
                                 IEnumerable<IFieldDefinition> fields,
                                 IEnumerable<IMessageDefinition> nestedMessages,
                                 IEnumerable<IEnumDefinition> nestedEnums,
                                 string documentation) : base(name, package, imports, documentation)
        {
            Fields = fields.ToList();
            NestedMessages = nestedMessages.ToList();
            NestedEnums = nestedEnums.ToList();
        }

        /// <summary>
        /// Create new instance of the <see cref="MessageDefinition"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public MessageDefinition(IMessageDefinition other) : base(other)
        {
            Fields = other.Fields.Select(x => new FieldDefinition(x)).Cast<IFieldDefinition>().ToList();
            NestedMessages = other.NestedMessages.Select(x => new MessageDefinition(x)).Cast<IMessageDefinition>().ToList();
            NestedEnums = other.NestedEnums.Select(x => new EnumDefinition(x)).Cast<IEnumDefinition>().ToList();
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as MessageDefinition;
            return other != null
                   && base.Equals(other)
                   && Fields.SequenceEquivalence(other.Fields)
                   && NestedMessages.SequenceEquivalence(other.NestedMessages)
                   && NestedEnums.SequenceEquivalence(other.NestedEnums);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (base.GetHashCode(),
                    Fields.CalcHashCode(),
                    NestedMessages.CalcHashCode(),
                    NestedEnums.CalcHashCode()).GetHashCode();
        }

        #endregion Object Overrides
    }
}
