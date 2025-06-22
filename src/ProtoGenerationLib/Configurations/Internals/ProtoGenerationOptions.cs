using ProtoGenerationLib.Configurations.Abstracts;

namespace ProtoGenerationLib.Configurations.Internals
{
    /// <inheritdoc cref="IProtoGenerationOptions"/>
    public class ProtoGenerationOptions : IProtoGenerationOptions
    {
        /// <summary>
        /// An instance of the <see cref="ProtoGenerationOptions"/> class
        /// that contains the default configurations.
        /// </summary>
        public static ProtoGenerationOptions Default { get; private set; }

        /// <inheritdoc cref="IProtoStylingConventionsStrategiesOptions"/>
        public ProtoStylingConventionsStrategiesOptions ProtoStylingConventionsStrategiesOptions { get; set; }
        IProtoStylingConventionsStrategiesOptions IProtoGenerationOptions.ProtoStylingConventionsStrategiesOptions => ProtoStylingConventionsStrategiesOptions;

        /// <inheritdoc cref="IProtoNamingStrategiesOptions"/>
        public ProtoNamingStrategiesOptions ProtoNamingStrategiesOptions { get; set; }
        IProtoNamingStrategiesOptions IProtoGenerationOptions.ProtoNamingStrategiesOptions => ProtoNamingStrategiesOptions;

        /// <inheritdoc cref="INumberingStrategiesOptions"/>
        public NumberingStrategiesOptions NumberingStrategiesOptions { get; set; }
        INumberingStrategiesOptions IProtoGenerationOptions.NumberingStrategiesOptions => NumberingStrategiesOptions;

        /// <inheritdoc cref="IAnalysisOptions"/>
        public AnalysisOptions AnalysisOptions { get; set; }
        IAnalysisOptions IProtoGenerationOptions.AnalysisOptions => AnalysisOptions;

        /// <inheritdoc cref="INewTypeNamingStrategiesOptions"/>
        public NewTypeNamingStrategiesOptions NewTypeNamingStrategiesOptions { get; set; }
        INewTypeNamingStrategiesOptions IProtoGenerationOptions.NewTypeNamingStrategiesOptions => NewTypeNamingStrategiesOptions;

        /// <inheritdoc/>
        public string ProtoFileSyntax { get; set; }

        /// <summary>
        /// Initialize the static members of the <see cref="ProtoGenerationOptions"/> class.
        /// </summary>
        static ProtoGenerationOptions()
        {
            Default = new ProtoGenerationOptions();
        }

        /// <summary>
        /// Create new instance of the <see cref="ProtoGenerationOptions"/> class.
        /// </summary>
        /// <param name="protoStylingConventionsStrategiesOptions"><inheritdoc cref="ProtoStylingConventionsStrategiesOptions" path="/node()"/><br/> Default to null converted to the default <see cref="Internals.ProtoStylingConventionsStrategiesOptions"/>.</param>
        /// <param name="protoNamingStrategiesOptions"><inheritdoc cref="ProtoNamingStrategiesOptions" path="/node()"/><br/> Default to null converted to the default <see cref="Internals.ProtoNamingStrategiesOptions"/>.</param>
        /// <param name="numberingStrategiesOptions"><inheritdoc cref="NumberingStrategiesOptions" path="/node()"/><br/> Default to null converted to the default <see cref="Internals.NumberingStrategiesOptions"/>.</param>
        /// <param name="analysisOptions"><inheritdoc cref="AnalysisOptions" path="/node()"/><br/> Default to null converted to the default <see cref="Internals.AnalysisOptions"/>.</param>
        /// <param name="newTypeNamingStrategiesOptions"><inheritdoc cref="NewTypeNamingStrategiesOptions" path="/node()"/><br/> Default to null converted to the default <see cref="Internals.NewTypeNamingStrategiesOptions"/>.</param>
        /// <param name="protoFileSyntax"><inheritdoc cref="ProtoFileSyntax" path="/node()"/><br/> Default to "proto3".</param>
        public ProtoGenerationOptions(ProtoStylingConventionsStrategiesOptions? protoStylingConventionsStrategiesOptions = null,
                                      ProtoNamingStrategiesOptions? protoNamingStrategiesOptions = null,
                                      NumberingStrategiesOptions? numberingStrategiesOptions = null,
                                      AnalysisOptions? analysisOptions = null,
                                      NewTypeNamingStrategiesOptions? newTypeNamingStrategiesOptions = null,
                                      string protoFileSyntax = "proto3")
        {
            ProtoStylingConventionsStrategiesOptions = protoStylingConventionsStrategiesOptions ?? new ProtoStylingConventionsStrategiesOptions();
            ProtoNamingStrategiesOptions = protoNamingStrategiesOptions ?? new ProtoNamingStrategiesOptions();
            NumberingStrategiesOptions = numberingStrategiesOptions ?? new NumberingStrategiesOptions();
            AnalysisOptions = analysisOptions ?? new AnalysisOptions();
            NewTypeNamingStrategiesOptions = newTypeNamingStrategiesOptions ?? new NewTypeNamingStrategiesOptions();
            ProtoFileSyntax = protoFileSyntax;
        }
    }
}
