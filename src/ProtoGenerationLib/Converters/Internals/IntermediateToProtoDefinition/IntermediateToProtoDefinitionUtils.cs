using System;
using System.Linq;

namespace ProtoGenerationLib.Converters.Internals.IntermediateToProtoDefinition
{
    /// <summary>
    /// Common utility functions for the converting intermediate proto representation
    /// to proto definition.
    /// </summary>
    public static class IntermediateToProtoDefinitionUtils
    {
        /// <summary>
        /// Retrieves the short name of the given <paramref name="typeFullName"/>
        /// in the file whose package is the given <paramref name="filePackage"/>.
        /// i.e if the <paramref name="filePackage"/> is "a.b" and the
        /// <paramref name="typeFullName"/> is "a.c" then the short name will be
        /// "c" and if the full name is "a.b.c.d" then the short name will be "c.d".
        /// </summary>
        /// <param name="typeFullName">The full name of the type whose short name is requested.</param>
        /// <param name="filePackage">The package of the file in which the short name can be used.</param>
        /// <param name="packageComponentsSeparator">The string that separate the package components.</param>
        /// <returns>
        /// The short name of the given <paramref name="typeFullName"/>
        /// in the file whose package is the given <paramref name="filePackage"/>.
        /// </returns>
        public static string GetTypeShortName(string typeFullName,
                                              string filePackage,
                                              string packageComponentsSeparator)
        {
            var typeFullNameComponents = typeFullName.Split(new string[] { packageComponentsSeparator }, StringSplitOptions.None);
            var packageComponents = filePackage.Split(new string[] { packageComponentsSeparator }, StringSplitOptions.None);

            var minNumOfComponents = Math.Min(typeFullNameComponents.Length, packageComponents.Length);
            int i = 0;
            while (i < minNumOfComponents)
            {
                if (typeFullNameComponents[i].Equals(packageComponents[i]))
                    i++;
                else
                    break;
            }

            return string.Join(packageComponentsSeparator, typeFullNameComponents.Skip(i));
        }
    }
}
