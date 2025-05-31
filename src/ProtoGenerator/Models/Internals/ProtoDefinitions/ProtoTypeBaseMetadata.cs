using ProtoGenerator.Models.Abstracts.ProtoDefinitions;
using System.Collections.Generic;

namespace ProtoGenerator.Models.Internals.ProtoDefinitions
{
    /// <inheritdoc cref="IProtoTypeBaseMetadata"/>
    public class ProtoTypeBaseMetadata : IProtoTypeBaseMetadata
    {
        /// <inheritdoc/>
        public string? Name { get; set; }

        /// <inheritdoc/>
        public string? Package { get; set; }

        /// <inheritdoc/>
        public string? FilePath { get; set; }

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="ProtoTypeBaseMetadata"/> class.
        /// </summary>
        public ProtoTypeBaseMetadata()
        {
            Name = null;
            Package = null;
            FilePath = null;
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoTypeBaseMetadata"/> class.
        /// </summary>
        /// <param name="name"><inheritdoc cref="Name" path="/node()"/></param>
        /// <param name="package"><inheritdoc cref="Package" path="/node()"/></param>
        /// <param name="filePath"><inheritdoc cref="FilePath" path="/node()"/></param>
        public ProtoTypeBaseMetadata(string name, string package, string filePath)
        {
            Name = name;
            Package = package;
            FilePath = filePath;
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoTypeBaseMetadata"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public ProtoTypeBaseMetadata(IProtoTypeBaseMetadata other)
        {
            Name = other.Name;
            Package = other.Package;
            FilePath = other.FilePath;
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as ProtoTypeBaseMetadata;
            return other != null
                   && EqualityComparer<string?>.Default.Equals(Name, other.Name)
                   && EqualityComparer<string?>.Default.Equals(Package, other.Package)
                   && EqualityComparer<string?>.Default.Equals(FilePath, other.FilePath);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return (Name,
                    Package,
                    FilePath).GetHashCode();
        }

        #endregion Object Overrides
    }
}
