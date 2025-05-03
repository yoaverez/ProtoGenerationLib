using ProtoGenerator.Configurations.Abstracts;
using System;

namespace ProtoGenerator.Configurations.Internals
{
    /// <inheritdoc cref="IAnalysisOptions"/>
    public class AnalysisOptions : IAnalysisOptions
    {
        /// <inheritdoc/>
        public bool IncludeFields { get; set; }

        /// <inheritdoc/>
        public bool IncludePrivates { get; set; }

        /// <inheritdoc/>
        public bool IncludeStatics { get; set; }

        /// <inheritdoc/>
        public string FieldsAndPropertiesExtractionStrategy { get; set; }

        /// <inheritdoc/>
        public Type IgnoreFieldOrPropertyAttribute { get; set; }

        /// <inheritdoc/>
        public Type DataTypeConstructorAttribute { get; set; }
    }
}
