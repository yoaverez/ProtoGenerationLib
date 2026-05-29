using System;

namespace ProtoGenerationLib.Attributes
{
    /// <summary>
    /// Attribute for marking data members as optional members in the proto message.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter)]
    public class OptionalDataMemberAttribute : Attribute
    {
    }
}
