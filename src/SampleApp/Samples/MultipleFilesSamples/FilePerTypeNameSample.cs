using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;
using ProtoGenerationLib.ProvidersAndRegistries.External;
using ProtoGenerationLib;
using SampleApp.Samples.MultipleFilesSamples.Dtos.CustomerService;
using SampleApp.Samples.MultipleFilesSamples.Dtos.GameDtos;

namespace SampleApp.Samples.MultipleFilesSamples
{
    public class FilePerTypeNameSample : ISampleRunner
    {
        public void RunSample()
        {
            var sampleTypes = new Type[] { typeof(ICustomerService), typeof(IGameService) };
            var baseFilePaths = $"{Common.BASE_PROTO_PATH}/{nameof(MultipleFilesSamples)}/{GetType().Name}";

            var generationOptions = new ProtoGenerationOptions();
            var protoGenerator = new ProtoGenerator();

            // Set the result package
            // to prevent collisions between samples.
            Common.SetPackageName(GetType(), protoGenerator.Registry, generationOptions);

            // Set the file naming strategy to type name.
            generationOptions.ProtoNamingStrategiesOptions.FileNamingStrategy =
                StrategyNamesLookup.FilePathStrategiesLookup[FilePathStrategyKind.TypeName];

            protoGenerator.GenerateProtos(sampleTypes, generationOptions)
                          .WriteToFiles(Common.PATH_TO_PROTO_ROOT, baseFilePaths);
        }
    }
}
