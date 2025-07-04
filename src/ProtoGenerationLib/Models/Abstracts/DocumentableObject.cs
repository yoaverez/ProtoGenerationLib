namespace ProtoGenerationLib.Models.Abstracts
{
    /// <summary>
    /// A base class for all the documentable objects.
    /// </summary>
    public abstract class DocumentableObject : IDocumentable
    {
        /// <inheritdoc/>
        public string Documentation { get; set; }

        #region Constructors

        /// <summary>
        /// Create new instance of the <see cref="DocumentableObject"/> class.
        /// </summary>
        /// <param name="documentation"><inheritdoc cref="Documentation" path="/node()"/> Default to <see cref="string.Empty"/>.</param>
        public DocumentableObject(string documentation = "")
        {
            Documentation = documentation;
        }

        /// <summary>
        /// Create new instance of the <see cref="DocumentableObject"/> class
        /// which is a copy of the given <paramref name="other"/>.
        /// </summary>
        /// <param name="other">The object to copy.</param>
        public DocumentableObject(IDocumentable other)
        {
            Documentation = other.Documentation;
        }

        #endregion Constructors

        #region Object Overrides

        /// <inheritdoc/>
        public override bool Equals(object? obj)
        {
            var other = obj as DocumentableObject;
            return other != null
                   && Documentation.Equals(other.Documentation);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return Documentation.GetHashCode();
        }

        #endregion Object Overrides
    }
}
