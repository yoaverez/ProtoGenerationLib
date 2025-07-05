using ProtoGenerationLib;
using ProtoGenerationLib.Configurations.Internals;

namespace SampleApp.Samples.SingleTypeSamples
{
    public class InheritanceSample : ISampleRunner
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
