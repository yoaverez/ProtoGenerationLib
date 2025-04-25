namespace ProtoGenerator.Strategies.Abstracts
{
    /// <summary>
    /// A strategy for styling words as protobuf words styles.
    /// </summary>
    public interface IProtoStylingStrategy
    {
        /// <summary>
        /// Convert the given <paramref name="word"/> to proto style.
        /// </summary>
        /// <param name="word">The word to convert to proto style.</param>
        /// <returns>The proto style word of the given <paramref name="word"/>.</returns>
        string ToProtoStyle(string word);

        /// <summary>
        /// Convert the given <paramref name="words"/> to proto style.
        /// </summary>
        /// <param name="words">The words to convert to proto style.</param>
        /// <returns>The proto style word of the given <paramref name="words"/>.</returns>
        string ToProtoStyle(string[] words);
    }
}
