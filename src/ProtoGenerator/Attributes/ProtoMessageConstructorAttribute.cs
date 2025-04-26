using System;

namespace ProtoGenerator.Attributes
{
    /// <summary>
    /// Attribute to mark a constructor as a message constructor.
    /// i.e. a constructor that gets all the public fields and properties of the class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
    public class ProtoMessageConstructorAttribute : Attribute
    {
    }
}
