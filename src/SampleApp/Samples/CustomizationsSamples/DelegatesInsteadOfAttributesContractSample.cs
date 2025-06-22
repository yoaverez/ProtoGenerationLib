using ProtoGenerationLib;
using ProtoGenerationLib.Attributes;
using ProtoGenerationLib.Configurations.Internals;
using System.Reflection;

namespace SampleApp.Samples.CustomizationsSamples
{
    public class DelegatesInsteadOfAttributesContractSample : ISampleRunner
    {
        public interface IContractTypeWithoutAttributes
        {
            public void Method1();

            public string Method2(bool b);

            public TimeSpan Method3(int a, object b, Guid c);

            public TimeSpan Method4(TimeSpan timeSpan);
        }

        public void RunSample()
        {
            var sampleType = typeof(IContractTypeWithoutAttributes);
            var baseFilePaths = $"{Common.BASE_PROTO_PATH}/{nameof(CustomizationsSamples)}";

            var generationOptions = new ProtoGenerationOptions();
            var protoGenerator = new ProtoGenerator();

            // Set the result file name and package
            // to prevent collisions between samples.
            Common.SetFileName(GetType(), protoGenerator.Registry, generationOptions);
            Common.SetPackageName(GetType(), protoGenerator.Registry, generationOptions);

            // Make sure that the type is considered service with rpcs.
            generationOptions.AnalysisOptions.IsProtoServiceDelegate = (type) => type.Equals(sampleType);
            generationOptions.AnalysisOptions.TryGetRpcTypeDelegate = (Type serviceType, MethodInfo method, out ProtoRpcType rpcType) =>
            {
                rpcType = default;
                if (serviceType.Equals(sampleType))
                {
                    if (method.Name.Equals(nameof(IContractTypeWithoutAttributes.Method1)))
                    {
                        rpcType = ProtoRpcType.Unary;
                        return true;
                    }

                    if (method.Name.Equals(nameof(IContractTypeWithoutAttributes.Method3)))
                    {
                        rpcType = ProtoRpcType.ClientStreaming;
                        return true;
                    }
                }

                return false;
            };

            protoGenerator.GenerateProtos(new Type[] { sampleType }, generationOptions)
                          .WriteToFiles(Common.PATH_TO_PROTO_ROOT, baseFilePaths);
        }
    }
}
