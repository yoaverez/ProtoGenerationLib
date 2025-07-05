using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib;
using ProtoGenerationLib.ProvidersAndRegistries.External;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;

namespace SampleApp.Samples.CustomizationsSamples
{
    public class FlattenExtractionStrategySample : ISampleRunner
    {
        public class Animal
        {
            public string Name { get; set; }
        }

        public class Dog : Animal
        {
            public string Breed { get; set; }
        }

        public void RunSample()
        {
            var sampleType = typeof(Dog);
            var baseFilePaths = $"{Common.BASE_PROTO_PATH}/{nameof(CustomizationsSamples)}";

            var generationOptions = new ProtoGenerationOptions();
            var protoGenerator = new ProtoGenerator();

            // Set the result file name and package
            // to prevent collisions between samples.
            Common.SetFileName(GetType(), protoGenerator.Registry, generationOptions);
            Common.SetPackageName(GetType(), protoGenerator.Registry, generationOptions);

            // Set the extraction strategy to flatten.
            generationOptions.AnalysisOptions.FieldsAndPropertiesExtractionStrategy =
                StrategyNamesLookup.FieldsAndPropertiesExtractionStrategiesLookup[FieldsAndPropertiesExtractionStrategyKind.Flatten];

            protoGenerator.GenerateProtos(new Type[] { sampleType }, generationOptions)
                          .WriteToFiles(Common.PATH_TO_PROTO_ROOT, baseFilePaths);
        }
    }
}
