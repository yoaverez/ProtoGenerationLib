using System;

namespace ProtoGenerator.Attributes
{
    /// <summary>
    /// Attribute to mark fields and properties
    /// as members that needs to be ignored.
    /// </summary>
    /// <remarks>
    /// If this attribute target a property, the backfield that csharp generates automatically,
    /// will also be ignored.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = false, Inherited = true)]
    public class ProtoIgnoreAttribute : Attribute
    {
    }
}
