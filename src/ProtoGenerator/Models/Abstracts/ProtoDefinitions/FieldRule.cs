namespace ProtoGenerator.Models.Abstracts.ProtoDefinitions
{
    /// <summary>
    /// Represents the rule for a field in a Protocol Buffer message.
    /// </summary>
    public enum FieldRule
    {
        /// <summary>
        /// No specific rule (default in proto3).
        /// </summary>
        None,

        /// <summary>
        /// Field is optional.
        /// </summary>
        Optional,

        /// <summary>
        /// Field is repeated (collection).
        /// </summary>
        Repeated
    }
}
