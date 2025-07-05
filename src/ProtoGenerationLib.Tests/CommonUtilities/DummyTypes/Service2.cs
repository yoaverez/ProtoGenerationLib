using ProtoGenerationLib.Attributes;

namespace ProtoGenerationLib.Tests.CommonUtilities.DummyTypes
{
    [ProtoService]
    internal class Service2 : Service1, IService1, IService5
    {
        public void IService1Method1()
        {
            throw new NotImplementedException();
        }

        public void IService1Method2()
        {
            throw new NotImplementedException();
        }

        [ProtoRpc(ProtoRpcType.ServerStreaming)]
        public void IService5Method1()
        {
            throw new NotImplementedException();
        }
    }
}
