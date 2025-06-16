namespace ProtoGenerationLib.Strategies.Abstracts
{
    /// <summary>
    /// A strategy for styling package names.
    /// </summary>
    public interface IPackageStylingStrategy : IProtoStylingStrategy
    {
        /// <summary>
        /// The string that separate between the package components.
        /// e.g if it is "." and the components are { "a", "b" } then
        /// the package will be "a.b".
        /// </summary>
        string PackageComponentsSeparator { get; }
    }
}
