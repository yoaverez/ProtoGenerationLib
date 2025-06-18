namespace ProtoGenerationLib.Strategies.Abstracts
{
    /// <summary>
    /// A strategy for styling file paths.
    /// </summary>
    public interface IFilePathStylingStrategy
    {
        /// <summary>
        /// Convert the given <paramref name="words"/> to proto style.
        /// </summary>
        /// <param name="words">The words to convert to proto style.</param>
        /// <returns>The proto style word of the given <paramref name="words"/>.</returns>
        string ToProtoStyle(string[] words);
    }
}
