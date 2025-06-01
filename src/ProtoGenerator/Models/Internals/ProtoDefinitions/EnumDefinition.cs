using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using ProtoGenerator.Utilities.CollectionUtilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerator.Models.Internals.ProtoDefinitions
{
    /// <inheritdoc cref="IEnumDefinition"/>
    public class EnumDefinition : ProtoObject, IEnumDefinition
    {
        /// <inheritdoc cref="IEnumDefinition.Values"/>
        public List<IEnumValueDefinition> Values { get; set; }
        /// <inheritdoc/>
        IEnumerable<IEnumValueDefinition> IEnumDefinition.Values => Values.AsReadOnly();

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="EnumDefinition"/> class.
        /// </summary>
        public EnumDefinition() : base()
        {
            Values = new List<IEnumValueDefinition>();
        }

        /// <summary>
        /// Create new instance of the <see cref="EnumDefinition"/> class.
        /// </summary>
        /// <param name="name"><inheritdoc cref="ProtoObject.Name" path="/node()"/></param>
        /// <param name="package"><inheritdoc cref="ProtoObject.Package" path="/node()"/></param>
        /// <param name="values"><inheritdoc cref="Values" path="/node()"/></param>
        public EnumDefinition(string name, string package, IEnumerable<IEnumValueDefinition> values) : base(name, package, Array.Empty<string>())
        {
            Values = values.ToList();
        }

        /// <summary>
        /// Create new instance of the <see cref="EnumDefinition"/>
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public EnumDefinition(IEnumDefinition other) : base(other)
        {
            Name = other.Name;
            Values = other.Values.Select(x => new EnumValueDefinition(x)).Cast<IEnumValueDefinition>().ToList();
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as EnumDefinition;
            return other != null
                   && base.Equals(other)
                   && Values.SequenceEquivalence(other.Values);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (base.GetHashCode(),
                    Values.CalcHashCode()).GetHashCode();
        }

        #endregion Object Overrides
    }
}
