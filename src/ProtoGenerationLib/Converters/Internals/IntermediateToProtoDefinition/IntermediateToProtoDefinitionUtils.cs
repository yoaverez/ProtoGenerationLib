using System;
using System.Linq;

namespace ProtoGenerationLib.Converters.Internals.IntermediateToProtoDefinition
{
    /// <summary>
    /// Common utility functions for the converting intermediate proto representation
    /// to proto definition.
    /// </summary>
    internal static class IntermediateToProtoDefinitionUtils
    {
        /// <summary>
        /// Retrieves the short name of the given <paramref name="innerTypeFullName"/>
        /// in the type whose full name is the given <paramref name="outerTypeFullName"/>.
        /// i.e if the <paramref name="outerTypeFullName"/> is "a.b" and the
        /// <paramref name="innerTypeFullName"/> is "a.c" then the short name will be
        /// "c" and if the full name is "a.b.c.d" then the short name will be "c.d".
        /// </summary>
        /// <param name="innerTypeFullName">The full name of the type whose short name is requested.</param>
        /// <param name="outerTypeFullName">The full name of the type that uses the type whose full name is the given <paramref name="innerTypeFullName"/>.</param>
        /// <param name="packageComponentsSeparator">The string that separate the package components.</param>
        /// <returns>
        /// The short name of the given <paramref name="innerTypeFullName"/>
        /// in the type whose full name is the given <paramref name="outerTypeFullName"/>.
        /// </returns>
        public static string GetTypeShortName(string innerTypeFullName,
                                              string outerTypeFullName,
                                              string packageComponentsSeparator)
        {
            var innerTypeFullNameComponents = innerTypeFullName.Split(new string[] { packageComponentsSeparator }, StringSplitOptions.None);
            var outerTypeFullNameComponents = outerTypeFullName.Split(new string[] { packageComponentsSeparator }, StringSplitOptions.None);

            var minNumOfComponents = Math.Min(innerTypeFullNameComponents.Length, outerTypeFullNameComponents.Length);
            int i = 0;
            while (i < minNumOfComponents)
            {
                if (innerTypeFullNameComponents[i].Equals(outerTypeFullNameComponents[i]))
                    i++;
                else
                    break;
            }

            // Type contains itself (like a Node struct).
            if (i == innerTypeFullNameComponents.Length)
                i--;

            return string.Join(packageComponentsSeparator, innerTypeFullNameComponents.Skip(i));
        }
    }
}
