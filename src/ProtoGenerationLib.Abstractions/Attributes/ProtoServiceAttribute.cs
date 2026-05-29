using System;

namespace ProtoGenerationLib.Attributes
{
    /// <summary>
    /// Attribute to mark a type as a type that represents a proto service.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class | AttributeTargets.Struct,
                    AllowMultiple = false, Inherited = true)]
    public class ProtoServiceAttribute : Attribute
    {
    }
}
