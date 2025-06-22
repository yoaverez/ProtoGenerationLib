using ProtoGenerationLib;
using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.Strategies.Internals.FileNamingStrategies;

namespace SampleApp.Samples.SingleTypeSamples
{
    public class ContractTypeSample : ISampleRunner
    {
        [ProtoService]
        public interface IContractType
        {
            [ProtoRpc(ProtoRpcType.Unary)]
            public void Method1();

            [ProtoRpc(ProtoRpcType.ServerStreaming)]
            public string Method2(bool b);

            [ProtoRpc(ProtoRpcType.ClientStreaming)]
            public TimeSpan Method3(int a, object b, Guid c);

            [ProtoRpc(ProtoRpcType.BidirectionalStreaming)]
            public TimeSpan Method4(TimeSpan timeSpan);
        }

        public void RunSample()
        {
            var sampleType = typeof(IContractType);
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
