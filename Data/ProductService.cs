using System.Text.Json;

namespace BisleriumCafe.Data
{
    public static class ProductService
    {
        public static void SaveItem(List<Product> items)
        {
            string itemFilePath = Utils.GetItemsPath();

            var json = JsonSerializer.Serialize(items);
            File.WriteAllText(itemFilePath, json);
        }

        public static List<Product> GetProducts()
        {
            string itemFilePath = Utils.GetItemsPath();

            if (!File.Exists(itemFilePath))
            {
                return new List<Product>();
            }

            var json = File.ReadAllText(itemFilePath);
            var items = JsonSerializer.Deserialize<List<Product>>(json);
            return items;
        }

        public static List<Product> CreateNewProduct(string itemName, float itemPrice, ProductType itemType)
        {
            List<Product> items = GetProducts();
            bool itemExists = items.Any(x => x.ProductName == itemName);

            if (itemExists)
            {
                throw new Exception($"{itemName} already exists!");
            }
            items.Add(
                new Product
                {
                    ProductName = itemName,
                    ProductPrice = itemPrice,
                    ProductType = itemType
                }
            );
            SaveItem(items);
            return items;
        }

        public static List<Product> UpdatePrice(string item_name, float item_price)
        {
            List<Product> items = GetProducts();
            Product item = items.FirstOrDefault(items => items.ProductName == item_name);

            if (item == null)
            {
                throw new Exception("This item could not be found!");
            }
            item.ProductPrice = item_price;
            SaveItem(items);
            return items;
        }

        public static List<Product> DeleteProduct(string item_name)
        {
            List<Product> items = GetProducts();
            Product item = items.FirstOrDefault(items => items.ProductName == item_name);

            if (item == null)
            {
                throw new Exception("This item could not be found!");
            }
            items.Remove(item);
            SaveItem(items);
            return items;
        }
    }
}
