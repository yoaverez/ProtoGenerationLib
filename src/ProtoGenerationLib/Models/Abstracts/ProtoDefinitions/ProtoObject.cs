﻿using ProtoGenerationLib.Utilities.CollectionUtilities;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib.Models.Abstracts.ProtoDefinitions
{
    /// <inheritdoc cref="IProtoObject"/>
    public abstract class ProtoObject : DocumentableObject, IProtoObject
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public string Package { get; set; }

        /// <inheritdoc cref="IProtoObject.Imports"/>
        public HashSet<string> Imports { get; set; }
        ISet<string> IProtoObject.Imports => Imports;

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="ProtoObject"/> class.
        /// </summary>
        public ProtoObject()
        {
            Imports = new HashSet<string>();
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoObject"/> class.
        /// </summary>
        /// <param name="name"><inheritdoc cref="Name" path="/node()"/></param>
        /// <param name="package"><inheritdoc cref="Package" path="/node()"/></param>
        /// <param name="imports"><inheritdoc cref="Imports" path="/node()"/></param>
        public ProtoObject(string name, string package, IEnumerable<string> imports)
        {
            Name = name;
            Package = package;
            Imports = imports.ToHashSet();
        }

        /// <inheritdoc cref="ProtoObject(string, string, IEnumerable{string})"/>
        /// <param name="documentation"><inheritdoc cref="DocumentableObject(string)" path="/param"/></param>
        public ProtoObject(string name, string package, IEnumerable<string> imports, string documentation) : base(documentation)
        {
            Name = name;
            Package = package;
            Imports = imports.ToHashSet();
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoObject"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public ProtoObject(IProtoObject other) : base(other)
        {
            Name = other.Name;
            Package = other.Package;
            Imports = other.Imports.ToHashSet();
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as ProtoObject;
            return other != null
                   && base.Equals(other)
                   && Name.Equals(other.Name)
                   && Package.Equals(other.Package)
                   && Imports.SequenceEquivalence(other.Imports);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (base.GetHashCode(),
                    Name,
                    Package,
                    Imports.CalcHashCode()).GetHashCode();
        }

        #endregion Object Overrides
    }
}
