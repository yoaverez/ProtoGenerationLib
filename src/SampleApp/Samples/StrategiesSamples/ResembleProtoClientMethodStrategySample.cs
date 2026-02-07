using ProtoGenerationLib;
using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Configurations.Internals;
using ProtoGenerationLib.ProvidersAndRegistries.External;
using ProtoGenerationLib.ProvidersAndRegistries.External.StrategiesNamesEnums;

namespace SampleApp.Samples.StrategiesSamples
{
    public class ResembleProtoClientMethodStrategySample : ISampleRunner
    {
        [ProtoService]
        public interface IContractType
        {
            [ProtoRpc(ProtoRpcType.Unary)]
            public Task Method1(DateTime? deadline, CancellationToken? cancellationToken);

            [ProtoRpc(ProtoRpcType.ServerStreaming)]
            public Task<int> Method2(bool a, DateTime? deadline, CancellationToken? cancellationToken);
        }
        public void RunSample()
        {
            var sampleType = typeof(IContractType);
            var baseFilePaths = $"{Common.BASE_PROTO_PATH}/{nameof(StrategiesSamples)}";

            var generationOptions = new ProtoGenerationOptions();
            var protoGenerator = new ProtoGenerator();

            // Set the result file name and package
            // to prevent collisions between samples.
            Common.SetFileName(GetType(), protoGenerator.Registry, generationOptions);
            Common.SetPackageName(GetType(), protoGenerator.Registry, generationOptions);

            generationOptions.AnalysisOptions.MethodSignatureExtractionStrategy =
                StrategyNamesLookup.MethodSignatureExtractionStrategiesLookup[MethodSignatureExtractionStrategyKind.ResembleProtoClientMethod];

            protoGenerator.GenerateProtos(new Type[] { sampleType }, generationOptions)
                          .WriteToFiles(Common.PATH_TO_PROTO_ROOT, baseFilePaths);
        }
    }
}
