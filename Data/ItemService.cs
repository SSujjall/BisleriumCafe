using System.Text.Json;

namespace BisleriumCafe.Data
{
    public static class ItemService
    {
        //
        public static void SaveItem(List<Item> items)
        {
            string itemFilePath = Utils.GetItemsPath();

            var json = JsonSerializer.Serialize(items);
            File.WriteAllText(itemFilePath, json);
        }

        public static List<Item> GetItems()
        {
            string itemFilePath = Utils.GetItemsPath();

            if (!File.Exists(itemFilePath))
            {
                return new List<Item>();
            }
            var json = File.ReadAllText(itemFilePath);
            var items = JsonSerializer.Deserialize<List<Item>>(json);
            return items;
        }

        public static List<Item> CreateItem(string iName, float iPrice, ProductType iType)
        {
            List<Item> items = GetItems();
            bool itemExist = items.Any(x => x.ItemName == iName);

            if (itemExist)
            {
                throw new Exception($"{iName} already exists");
            }
            items.Add(
                new Item
                {
                    ItemName = iName,
                    ItemPrice = iPrice,
                    ItemType = iType
                }
                );

            SaveItem(items);
            return items; //mathi save gareko items bolako
        }

        public static List<Item> UpdatePrice(string iName, float iPrice)
        {
            List<Item> items = GetItems();

            Item item = items.FirstOrDefault(x => x.ItemName == iName);

            if (item == null)
            {
                throw new Exception("Item Not Found!");
            }
            item.ItemPrice = iPrice;
            SaveItem(items);
            return items;
        }

        public static List<Item> DeleteItem(string iName)
        {
            List<Item> items = GetItems();
            Item item = items.FirstOrDefault(x => x.ItemName == iName);

            if (item == null)
            {
                throw new Exception("Item Not Found!");
            }

            items.Remove(item);
            SaveItem(items);
            return items;
        }
    }
}