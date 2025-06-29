using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib;
using ProtoGenerationLib.Attributes;

namespace SampleApp.Samples.CustomizationsSamples
{
    public class AddCustomDocumentationSample : ISampleRunner
    {
        [ProtoService]
        public interface IContractType
        {
            [ProtoRpc(ProtoRpcType.Unary)]
            public void Method1();

            [ProtoRpc(ProtoRpcType.Unary)]
            public string Method2(bool b);
        }

        public void RunSample()
        {
            var sampleType = typeof(IContractType);
            var baseFilePaths = $"{Common.BASE_PROTO_PATH}/{nameof(CustomizationsSamples)}";

            var generationOptions = new ProtoGenerationOptions();
            var protoGenerator = new ProtoGenerator();

            // Set the result file name and package
            // to prevent collisions between samples.
            Common.SetFileName(GetType(), protoGenerator.Registry, generationOptions);
            Common.SetPackageName(GetType(), protoGenerator.Registry, generationOptions);

            // Set the field suffixes.
            generationOptions.AddDocumentation<IContractType>($"The is a contract for the depiction of the proto generator{Environment.NewLine}" +
                                                              $"documentation customization.");

            generationOptions.AddDocumentation<IContractType>(nameof(IContractType.Method1), 0, "Method1 documentation.");
            generationOptions.AddDocumentation<IContractType>(nameof(IContractType.Method2), 1, "A second method that shows how the documentation customization works.");

            protoGenerator.GenerateProtos(new Type[] { sampleType }, generationOptions)
                          .WriteToFiles(Common.PATH_TO_PROTO_ROOT, baseFilePaths);
        }
    }
}
