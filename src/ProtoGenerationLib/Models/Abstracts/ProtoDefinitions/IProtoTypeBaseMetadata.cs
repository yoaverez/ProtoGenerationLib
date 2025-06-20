namespace ProtoGenerationLib.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// The base meta data of a proto type.
    /// </summary>
    public interface IProtoTypeBaseMetadata
    {
        /// <summary>
        /// The name of the proto type.
        /// </summary>
        string? Name { get; }

        /// <summary>
        /// The package that the proto type is define within.
        /// </summary>
        /// <remarks>
        /// <list type="number">
        /// <item><b>The package components should be separated by '.'.</b></item>
        /// <item><b>If this meta data is for primitive type, set this property to <see cref="string.Empty"/>.</b></item>
        /// </list>
        /// </remarks>
        string? Package { get; }

        /// <summary>
        /// The file relative path (relative to the project you ran this)
        /// in which this proto type is defined.
        /// </summary>
        /// <remarks>
        /// <list type="number">
        /// <item>
        /// <b>The file path components should be separated by
        /// forward slash (/).</b>
        /// </item>
        /// <item>
        /// <b>If you want to add an external proto add the
        /// <see cref="Constants.WellKnownTypesConstants.EXTERNAL_FILE_PATH_PREFIX"/>
        /// prefix to the file path.</b>
        /// </item>
        /// <item><b>If this meta data is for primitive type, set this property to <see cref="string.Empty"/>.</b></item>
        /// </list>
        /// </remarks>
        string? FilePath { get; }

        /// <summary>
        /// Whether or not a new proto type should be created.
        /// </summary>
        /// <remarks>
        /// This property should be false if and only if the type is well known type or primitive or
        /// if the proto type is already defined in an external file.
        /// </remarks>
        bool ShouldCreateProtoType { get; }
    }
}
