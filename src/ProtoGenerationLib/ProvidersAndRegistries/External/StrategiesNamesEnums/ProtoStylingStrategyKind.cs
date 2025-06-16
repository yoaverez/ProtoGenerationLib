namespace ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums
{
    /// <summary>
    /// Enum containing all the project defined
    /// proto styling strategies.
    /// </summary>
    public enum ProtoStylingStrategyKind
    {
        /// <summary>
        /// Format string as a camelCase string.
        /// </summary>
        CamelCase,

        /// <summary>
        /// Format string as a dot delimited snake case string.
        /// e.g if the words are: ILikeApple, MeToo then the
        /// result will be i_like_apple.me_too
        /// </summary>
        DotDelimitedSnakeCase,

        /// <summary>
        /// Format string as a snake_case string.
        /// </summary>
        SnakeCase,

        /// <summary>
        /// Format string as a UpperCamelCase string.
        /// </summary>
        UpperCamelCase,

        /// <summary>
        /// Format string as a UPPER_SNAKE_CASE string.
        /// </summary>
        UpperSnakeCase,
    }
}
