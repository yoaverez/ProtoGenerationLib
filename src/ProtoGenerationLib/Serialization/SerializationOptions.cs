namespace ProtoGenerationLib.Serialization
{
    /// <summary>
    /// Represents the proto generation serialization options.
    /// </summary>
    public class SerializationOptions : ISerializationOptions
    {
        /// <summary>
        /// An instance of the <see cref="SerializationOptions"/> containing the
        /// default values.
        /// </summary>
        public static SerializationOptions Default { get; set; }

        /// <inheritdoc/>
        public uint IndentSize { get; set; }

        /// <summary>
        /// Initialize the static members of the <see cref="SerializationOptions"/> class.
        /// </summary>
        static SerializationOptions()
        {
            Default = GetDefaultSerializationOptions();
        }

        /// <summary>
        /// Create new instance of the <see cref="SerializationOptions"/> class.
        /// </summary>
        /// <param name="indentSize"><inheritdoc cref="IndentSize" path="/node()"/></param>
        public SerializationOptions(uint indentSize)
        {
            IndentSize = indentSize;
        }

        /// <summary>
        /// Create new <see cref="SerializationOptions"/> instance that
        /// contains default values.
        /// </summary>
        /// <returns>
        /// A new <see cref="SerializationOptions"/> instance that
        /// contains default values.
        /// </returns>
        public static SerializationOptions GetDefaultSerializationOptions()
        {
            return new SerializationOptions(indentSize: 2);
        }
    }
}
