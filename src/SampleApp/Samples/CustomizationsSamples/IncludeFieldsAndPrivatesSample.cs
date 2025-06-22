using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;
using ProtoGenerationLib.ProvidersAndRegistries.External;
using ProtoGenerationLib;

namespace SampleApp.Samples.CustomizationsSamples
{
    public class IncludeFieldsAndPrivatesSample : ISampleRunner
    {
        public class ProductDto
        {
            public string Name { get; set; }

            private decimal _price;

            public decimal Price
            {
                get => _price;
                set => _price = value >= 0 ? value : throw new ArgumentException("Price cannot be negative.");
            }

            public bool IsExpensive => _price > 1000;

            private Guid _internalId = Guid.NewGuid();
        }

        public void RunSample()
        {
            var sampleType = typeof(ProductDto);
            var baseFilePaths = $"{Common.BASE_PROTO_PATH}/{nameof(CustomizationsSamples)}";

            var generationOptions = new ProtoGenerationOptions();
            var protoGenerator = new ProtoGenerator();

            // Set the result file name and package
            // to prevent collisions between samples.
            Common.SetFileName(GetType(), protoGenerator.Registry, generationOptions);
            Common.SetPackageName(GetType(), protoGenerator.Registry, generationOptions);

            // Include fields and privates.
            generationOptions.AnalysisOptions.IncludeFields = true;
            generationOptions.AnalysisOptions.IncludePrivates = true;

            protoGenerator.GenerateProtos(new Type[] { sampleType }, generationOptions)
                          .WriteToFiles(Common.PATH_TO_PROTO_ROOT, baseFilePaths);
        }
    }
}
