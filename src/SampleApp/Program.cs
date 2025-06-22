using SampleApp.Samples.CustomizationsSamples;
using SampleApp.Samples.MultipleFilesSamples;
using SampleApp.Samples.SingleTypeSamples;

namespace SampleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Single type samples.
            new PlainDataTypeSample().RunSample();
            new NestedClassSample().RunSample();
            new InheritanceSample().RunSample();
            new EnumSample().RunSample();
            new CollectionsSample().RunSample();
            new NullableTypeSample().RunSample();
            new TuplesSample().RunSample();
            new RecursiveStructureSample().RunSample();
            new ContractTypeSample().RunSample();

            // Customizations samples.
            new DelegatesInsteadOfAttributesContractSample().RunSample();
            new CustomConverterToChageDataTypeFields().RunSample();
            new CustomTypeMapperSample().RunSample();
            new AddFieldSuffixesSample().RunSample();
            new FlattenExtractionStrategySample().RunSample();
            new IncludeFieldsAndPrivatesSample().RunSample();

            // Multi files samples.
            new FilePerTypeNameSample().RunSample();
            new FilePerNameSpaceSample().RunSample();
            new FilePerNameSpaceAndTypeSample().RunSample();
        }
    }
}
