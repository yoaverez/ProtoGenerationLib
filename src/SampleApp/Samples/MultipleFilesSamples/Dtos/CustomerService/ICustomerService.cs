using ProtoGenerationLib.Attributes;
using Dtos.CustomerService.CustomerDtos;
using Dtos.CustomerService.OrdersDtos;

namespace Dtos.CustomerService
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
