namespace BisleriumCafe.Data
{
    public class Item
    {
        public required string ItemName { get; set; }
        public float ItemPrice { get; set; }
        public ProductType ItemType { get; set; }
    }
}
