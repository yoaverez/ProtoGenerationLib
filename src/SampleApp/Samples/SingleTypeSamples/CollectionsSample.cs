using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Strategies.Internals.FileNamingStrategies;
using ProtoGenerationLib;

namespace SampleApp.Samples.SingleTypeSamples
{
    public class CollectionsSample : ISampleRunner
    {
        public class Order
        {
            public List<string> Items { get; set; }
            public Dictionary<string, int> ItemQuantities { get; set; }
            public string[][][] StringJaggedArray {get; set;}
            public int[,,,,] IntMultidimensionalArray {get; set;}
        }

        public void RunSample()
        {
            var sampleType = typeof(Order);
            var baseFilePaths = $"{Common.BASE_PROTO_PATH}/{nameof(SingleTypeSamples)}";

            var generationOptions = new ProtoGenerationOptions();
            var protoGenerator = new ProtoGenerator();

            // Set the result file name and package
            // to prevent collisions between samples.
            Common.SetFileName(GetType(), protoGenerator.Registry, generationOptions);
            Common.SetPackageName(GetType(), protoGenerator.Registry, generationOptions);

            protoGenerator.GenerateProtos(new Type[] { sampleType }, generationOptions)
                          .WriteToFiles(Common.PATH_TO_PROTO_ROOT, baseFilePaths);
        }
    }
}
