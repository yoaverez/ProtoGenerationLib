namespace SampleApp.Samples.MultipleFilesSamples.Dtos.CustomerService.OrdersDtos
{
    public class Order
    {
        public int Id { get; set; }

        public long TotalPrice { get; set; }

        public IEnumerable<Item> Items { get; set; }
    }
}
