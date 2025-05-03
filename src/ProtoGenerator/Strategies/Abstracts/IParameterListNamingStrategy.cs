using System;
using System.Reflection;

namespace ProtoGenerator.Strategies.Abstracts
{
    /// <summary>
    /// A strategy for naming types that represents methods parameters list
    /// whose length is greater than 1.
    /// </summary>
    public interface IParameterListNamingStrategy
    {
        /// <summary>
        /// Get the name of a new type that represents a methods parameter list.
        /// </summary>
        /// <param name="methodInfo">The info of the method.</param>
        /// <returns>The name of a new type that represents a methods parameter list.</returns>
        string GetNewParametersListTypeName(MethodInfo methodInfo);
    }
}
