using ContractTypeSamplePack;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using System.Threading.Tasks;

namespace SampleApp.GeneratedProtos
{
    public class A : IContractType.IContractTypeBase
    {
        private IContractType.IContractTypeClient client;
        public void ClientMethod()
        {
            var a = client.Method4();
            a.RequestStream.WriteAsync();
            a.ResponseStream.CompleteAsync();

            var b = client.Method2();
            var c = client.Method3();
        }

        public override Task<Empty> Method1(Empty request, ServerCallContext context)
        {
            return base.Method1(request, context);
        }

        public override Task Method2(EnumTypeWrapper request, IServerStreamWriter<EnumTypeWrapper> responseStream, ServerCallContext context)
        {
            return base.Method2(request, responseStream, context);
        }

        public override Task<Duration> Method3(IAsyncStreamReader<Method3Int32ObjectGuidObject> requestStream, ServerCallContext context)
        {
            return base.Method3(requestStream, context);
        }

        public override Task Method4(IAsyncStreamReader<Duration> requestStream, IServerStreamWriter<Duration> responseStream, ServerCallContext context)
        {
            return base.Method4(requestStream, responseStream, context);
        }
    }
}
