using ProtoGenerationLib.Strategies.Internals.TypeNamingStrategies;
using System.Linq;
using System.Reflection;
using ProtoGenerationLib.Strategies.Abstracts;

namespace ProtoGenerationLib.Strategies.Internals.ParameterListNamingStrategies
{
    /// <summary>
    /// A parameter list naming strategy that names a parameter list
    /// based on the method name with a suffix.
    /// </summary>
    public class ParameterListNamingStrategy : IParameterListNamingStrategy
    {
        /// <summary>
        /// The strategy to use to name the types.
        /// </summary>
        ITypeNamingStrategy typeNamingStrategy;

        /// <summary>
        /// Create new instance of the <see cref="ParameterListNamingStrategy"/> class.
        /// </summary>
        public ParameterListNamingStrategy()
        {
            typeNamingStrategy = new TypeNameAsAlphaNumericTypeNameStrategy();
        }

        /// <inheritdoc/>
        public string GetNewParametersListTypeName(MethodInfo methodInfo)
        {
            var methodParametersTypes = methodInfo.GetParameters().Select(x => x.ParameterType);
            var methodParametersString = string.Join(string.Empty, methodParametersTypes.Select(typeNamingStrategy.GetTypeName));
            return $"{methodInfo.Name}{methodParametersString}";
        }
    }
}
