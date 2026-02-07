using ProtoGenerationLib.Replacers.Abstracts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProtoGenerationLib.Replacers.Internals.MethodSignatureTypeReplacers
{
    /// <summary>
    /// A replacer for a method return type or a parameters type
    /// of type <see cref="Task"/> or <see cref="Task{T}"/>.
    /// </summary>
    public class TaskMethodSignatureTypeReplacer : IMethodSignatureTypeReplacer
    {
        /// <inheritdoc/>
        public bool CanReplace(Type type, bool isReturnType)
        {
            return typeof(Task).IsAssignableFrom(type);
        }

        /// <inheritdoc/>
        public Type ReplaceType(Type type, bool isReturnType)
        {
            if (!CanReplace(type, isReturnType))
                throw new ArgumentException($"The given {nameof(type)}: {type.Name} can not be replaced by the {nameof(TaskMethodSignatureTypeReplacer)}.");

            var currentType = type;
            while (currentType != null)
            {
                // There is no reason to module a generic task in a proto.
                // It is equivalent to its result type.
                if (currentType.IsGenericType && currentType.GetGenericTypeDefinition().Equals(typeof(Task<>)))
                    return currentType.GetGenericArguments().Single();

                // There is no reason to module task in a proto.
                // It is equivalent to void.
                if (currentType.Equals(typeof(Task)))
                    return typeof(void);

                currentType = currentType.BaseType;
            }

            throw new ArgumentException($"Fatal Error: The given {nameof(type)}: {type.Name} is not a {nameof(Task)}.");
        }
    }
}
