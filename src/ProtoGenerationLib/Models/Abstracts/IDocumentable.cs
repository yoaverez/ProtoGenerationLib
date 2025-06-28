namespace ProtoGenerationLib.Models.Abstracts
{
    /// <summary>
    /// A contracts representing object that can have documentation.
    /// </summary>
    public interface IDocumentable
    {
        /// <summary>
        /// The documentation of this object.
        /// </summary>
        string Documentation { get; }
    }
}
