using ProtoGenerationLib.Replacers.Abstracts;
using ProtoGenerationLib.Utilities.TypeUtilities;
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
            return type.IsTask();
        }

        /// <inheritdoc/>
        public Type ReplaceType(Type type, bool isReturnType)
        {
            if (!CanReplace(type, isReturnType))
                throw new ArgumentException($"The given {nameof(type)}: {type.Name} can not be replaced by the {nameof(TaskMethodSignatureTypeReplacer)}.");

            if (!type.TryGetTaskElementType(out var taskType))
                throw new ArgumentException($"Fatal Error: The given {nameof(type)}: {type.Name} is not a {nameof(Task)}.");

            return taskType!;
        }
    }
}
