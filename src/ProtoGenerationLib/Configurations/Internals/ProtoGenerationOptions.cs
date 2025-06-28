using ProtoGenerationLib.Configurations.Abstracts;
using ProtoGenerationLib.Customizations.Abstracts;
using ProtoGenerationLib.Customizations.Internals;
using ProtoGenerationLib.Models.Abstracts.IntermediateRepresentations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProtoGenerationLib.Configurations.Internals
{
    /// <inheritdoc cref="IProtoGenerationOptions"/>
    public class ProtoGenerationOptions : IProtoGenerationOptions, IFieldSuffixRegister, IDocumentationAdder
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
        /// A collection containing all the contract type custom converters.
        /// </summary>
        /// <remarks>
        /// The first converter that can handle a type will be the only converter that
        /// will handle the type.
        /// </remarks>
        public IList<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>> ContractTypeCustomConverters { get; private set; }

        /// <summary>
        /// A collection containing all the data type custom converters.
        /// </summary>
        /// <remarks>
        /// The first converter that can handle a type will be the only converter that
        /// will handle the type.
        /// </remarks>
        public IList<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>> DataTypeCustomConverters { get; private set; }

        /// <summary>
        /// A collection containing all the enum type custom converters.
        /// </summary>
        /// <remarks>
        /// The first converter that can handle a type will be the only converter that
        /// will handle the type.
        /// </remarks>
        public IList<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>> EnumTypeCustomConverters { get; private set; }

        /// <summary>
        /// A collection containing all the custom type mappers.
        /// </summary>
        /// <remarks>
        /// The first mapper that can handle a type will be the only mapper that
        /// will handle the type.
        /// </remarks>
        public IList<ICustomTypeMapper> CustomTypeMappers { get; private set; }

        /// <inheritdoc cref="FieldSuffixProviderAndRegister"/>
        private FieldSuffixProviderAndRegister fieldSuffixProviderAndRegister;

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

            ContractTypeCustomConverters = new List<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>>();
            DataTypeCustomConverters = new List<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>>();
            EnumTypeCustomConverters = new List<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>>();
            CustomTypeMappers = new List<ICustomTypeMapper>();

            fieldSuffixProviderAndRegister = new FieldSuffixProviderAndRegister();
        }

        /// <inheritdoc/>
        public IEnumerable<ICustomTypesExtractor> GetCustomTypesExtractors()
        {
            return ContractTypeCustomConverters.Concat<ICustomTypesExtractor>(DataTypeCustomConverters)
                                               .Concat(EnumTypeCustomConverters)
                                               .ToArray();
        }

        /// <inheritdoc/>
        public IEnumerable<ICSharpToIntermediateCustomConverter<IContractTypeMetadata>> GetContractTypeCustomConverters()
        {
            return ContractTypeCustomConverters;
        }

        /// <inheritdoc/>
        public IEnumerable<ICSharpToIntermediateCustomConverter<IDataTypeMetadata>> GetDataTypeCustomConverters()
        {
            return DataTypeCustomConverters;
        }

        /// <inheritdoc/>
        public IEnumerable<ICSharpToIntermediateCustomConverter<IEnumTypeMetadata>> GetEnumTypeCustomConverters()
        {
            return EnumTypeCustomConverters;
        }

        /// <inheritdoc/>
        public IEnumerable<ICustomTypeMapper> GetCustomTypeMappers()
        {
            return CustomTypeMappers;
        }

        #region IFieldSuffixProvider Implementation

        /// <inheritdoc/>
        public bool TryGetFieldSuffix(Type fieldDeclaringType, Type fieldType, string fieldName, out string suffix)
        {
            return fieldSuffixProviderAndRegister.TryGetFieldSuffix(fieldDeclaringType, fieldType, fieldName, out suffix);
        }

        #endregion IFieldSuffixProvider Implementation

        #region IFieldSuffixRegister Implementation

        /// <inheritdoc/>
        public void AddFieldSuffix<TFieldType>(string suffix)
        {
            fieldSuffixProviderAndRegister.AddFieldSuffix<TFieldType>(suffix);
        }

        /// <inheritdoc/>
        public void AddFieldSuffix<TFieldDeclaringType>(string fieldName, string suffix)
        {
            fieldSuffixProviderAndRegister.AddFieldSuffix<TFieldDeclaringType>(fieldName, suffix);
        }

        /// <inheritdoc/>
        public void AddFieldSuffix<TFieldDeclaringType, TFieldType>(string suffix)
        {
            fieldSuffixProviderAndRegister.AddFieldSuffix<TFieldDeclaringType, TFieldType>(suffix);
        }

        /// <inheritdoc/>
        public void AddFieldThatShouldNotHaveSuffix<TFieldDeclaringType, TFieldType>(string fieldName)
        {
            fieldSuffixProviderAndRegister.AddFieldThatShouldNotHaveSuffix<TFieldDeclaringType, TFieldType>(fieldName);
        }

        #endregion IFieldSuffixRegister Implementation

        #region IDocumentationProvider Implementation

        /// <inheritdoc/>
        public bool TryGetTypeDocumentation(Type type, out string documentation)
        {
            return AnalysisOptions.TryGetTypeDocumentation(type, out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetFieldDocumentation(Type fieldDeclaringType, string fieldName, out string documentation)
        {
            return AnalysisOptions.TryGetFieldDocumentation(fieldDeclaringType, fieldName, out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetMethodDocumentation(Type methodDeclaringType, string methodName, int methodNumOfParams, out string documentation)
        {
            return AnalysisOptions.TryGetMethodDocumentation(methodDeclaringType, methodName, methodNumOfParams, out documentation);
        }

        /// <inheritdoc/>
        public bool TryGetEnumValueDocumentation(Type enumType, int enumValue, out string documentation)
        {
            return AnalysisOptions.TryGetEnumValueDocumentation(enumType, enumValue, out documentation);
        }

        #endregion IDocumentationProvider Implementation

        #region IDocumentationAdder Implementation

        /// <inheritdoc/>
        public void AddDocumentation<TType>(string documentation)
        {
            AnalysisOptions.AddDocumentation<TType>(documentation);
        }

        /// <inheritdoc/>
        public void AddDocumentation<TFieldDeclaringType>(string fieldName, string documentation)
        {
            AnalysisOptions.AddDocumentation<TFieldDeclaringType>(fieldName, documentation);
        }

        /// <inheritdoc/>
        public void AddDocumentation<TMethodDeclaringType>(string methodName, int numOfParameters, string documentation)
        {
            AnalysisOptions.AddDocumentation<TMethodDeclaringType>(methodName, numOfParameters, documentation);
        }

        /// <inheritdoc/>
        public void AddDocumentation<TEnumType>(int enumValue, string documentation) where TEnumType : Enum
        {
            AnalysisOptions.AddDocumentation<TEnumType>(enumValue, documentation);
        }

        #endregion IDocumentationAdder Implementation
    }
}
