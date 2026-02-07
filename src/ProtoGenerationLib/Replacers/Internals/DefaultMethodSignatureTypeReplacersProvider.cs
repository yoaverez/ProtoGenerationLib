using ProtoGenerationLib.Replacers.Abstracts;
using ProtoGenerationLib.Replacers.Internals.MethodSignatureTypeReplacers;
using System.Collections.Generic;

namespace ProtoGenerationLib.Replacers.Internals
{
    /// <summary>
    /// Responsible for providing the default method signature type replacers.
    /// </summary>
    public static class DefaultMethodSignatureTypeReplacersProvider
    {
        /// <summary>
        /// Get the default method signature type replacers.
        /// </summary>
        public static IEnumerable<IMethodSignatureTypeReplacer> GetDefaultMethodSignatureTypeReplacers()
        {
            return
            [
                new TaskMethodSignatureTypeReplacer(),
            ];
        }
    }
}
