using SampleApp.Samples.MultipleFilesSamples.Dtos.CustomerService.Generics;
using SampleApp.Samples.MultipleFilesSamples.Dtos.CustomerService.OrdersDtos;

namespace SampleApp.Samples.MultipleFilesSamples.Dtos.CustomerService.CustomerDtos
{
    public class CustomerInfo : Person
    {
        public enum CustomerRating
        {
            GoodCustomer = 7,
            BadCustomer = 2,
        }

        public CustomerRating Rating { get; set; }

        public Node<Order> Orders { get; set; }

        public Dictionary<int, Item> ShoppingCart { get; set; }
    }
}
