﻿using ProtoGenerationLib.Configurations.Abstracts;
using System;

namespace ProtoGenerationLib.Converters.Abstracts
{
    /// <summary>
    /// Converter between csharp type to the type's intermediate representation.
    /// </summary>
    /// <typeparam name="TIntermediate">The type of the intermediate representation object.</typeparam>
    internal interface ICSharpToIntermediateConverter<TIntermediate>
    {
        /// <summary>
        /// Convert the given <paramref name="type"/> to it's intermediate representation.
        /// </summary>
        /// <param name="type">The type to convert from csharp to intermediate representation.</param>
        /// <param name="generationOptions">The proto generation options.</param>
        /// <returns>The intermediate representation of the given <paramref name="type"/>.</returns>
        TIntermediate ConvertTypeToIntermediateRepresentation(Type type, IProtoGenerationOptions generationOptions);
    }
}
