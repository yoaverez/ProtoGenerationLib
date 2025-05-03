using System.Collections.Generic;
using System.Reflection;
using System;
using ProtoGenerator.Configurations.Abstracts;

namespace ProtoGenerator.Strategies.Abstracts
{
    /// <summary>
    /// Contract for extracting fields and properties from csharp type.
    /// </summary>
    public interface IFieldsAndPropertiesExtractionStrategy
    {
        /// <summary>
        /// Extract fields and properties from the given <paramref name="type"/>
        /// based on the given <paramref name="analysisOptions"/>.
        /// </summary>
        /// <param name="type">The type from which to extract fields and properties.</param>
        /// <param name="analysisOptions">Options for the extraction.</param>
        /// <returns>
        /// All the fields and properties infos from the given <paramref name="type"/>
        /// that are compatible with the given <paramref name="analysisOptions"/>.
        /// </returns>
        IEnumerable<(Type Type, string Name)> ExtractFieldsAndProperties(Type type, IAnalysisOptions analysisOptions);
    }
}
