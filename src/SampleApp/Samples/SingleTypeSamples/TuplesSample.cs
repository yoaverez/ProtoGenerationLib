using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Strategies.Internals.FileNamingStrategies;
using ProtoGenerationLib;

namespace SampleApp.Samples.SingleTypeSamples
{
    public class TuplesSample : ISampleRunner
    {
        public class TupleHolder
        {
            public (int, string) Pair { get; set; }
        }

        public void RunSample()
        {
            var sampleType = typeof(TupleHolder);
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
