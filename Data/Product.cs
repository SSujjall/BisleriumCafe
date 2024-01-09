namespace BisleriumCafe.Data
{
    public class Product
    {
        public required string ProductName { get; set; }

        public float ProductPrice { get; set; }

        public ProductType ProductType { get; set; }
    }
}
