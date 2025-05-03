using ProtoGenerator.Strategies.Abstracts;
using System.Reflection;

namespace ProtoGenerator.Strategies.Internals.PatameterListNamingStrategies
{
    /// <summary>
    /// A parameter list naming strategy that names a parameter list
    /// based on the method name with a suffix.
    /// </summary>
    public class ParameterListNamingStrategy : IParameterListNamingStrategy
    {
        /// <inheritdoc/>
        public string GetNewParametersListTypeName(MethodInfo methodInfo)
        {
            return $"{methodInfo.Name}ParameterList";
        }
    }
}
