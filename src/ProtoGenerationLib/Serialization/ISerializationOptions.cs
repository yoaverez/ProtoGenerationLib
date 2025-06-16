namespace ProtoGenerationLib.Serialization
{
    /// <summary>
    /// A contract representing the proto generation serialization options.
    /// </summary>
    public interface ISerializationOptions
    {
        /// <summary>
        /// The indentation size in spaces.
        /// e.g. IndentSize = x means that each tab/indentation is x spaces.
        /// </summary>
        uint IndentSize { get; }
    }
}
