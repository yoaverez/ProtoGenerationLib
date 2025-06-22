using ProtoGenerationLib.Attributes;
using SampleApp.Samples.MultipleFilesSamples.Dtos.CustomerService.CustomerDtos;
using SampleApp.Samples.MultipleFilesSamples.Dtos.CustomerService.OrdersDtos;

namespace SampleApp.Samples.MultipleFilesSamples.Dtos.CustomerService
{
    [ProtoService]
    public interface ICustomerService
    {
        [ProtoRpc(ProtoRpcType.Unary)]
        public CustomerInfo GetCustomerInfo(int customerId);

        [ProtoRpc(ProtoRpcType.BidirectionalStreaming)]
        public Item RequestItemsStream(int itemId);
    }
}
