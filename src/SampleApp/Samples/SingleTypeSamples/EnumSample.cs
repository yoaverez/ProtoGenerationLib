using ProtoGenerationLib;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Strategies.Internals.FileNamingStrategies;

namespace SampleApp.Samples.SingleTypeSamples
{
    public class EnumSample : ISampleRunner
    {
        public enum Status
        {
            Unknown = 0,
            Active = 1,
            Inactive = 2
        }

        public class Task
        {
            public string Title { get; set; }
            public Status CurrentStatus { get; set; }
        }

        public void RunSample()
        {
            var sampleType = typeof(Task);
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
