namespace BisleriumCafe.Data
{
    public class Order
    {
        public Guid TransactionID { get; set; } = Guid.NewGuid();

        public required List<Product> Products { get; set; }

        public long MemberID { get; set; }

        public DateTime DateOfSale {  get; set; }

        public float TotalPrice {  get; set; }
    }
}
