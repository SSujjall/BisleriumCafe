using System.Text.Json;

namespace BisleriumCafe.Data
{
    public static class ItemService
    {
        public static void SaveItem(List<Item> items)
        {
            string itemFilePath = Utils.GetItemsFilePath();

            var json = JsonSerializer.Serialize(items);
            File.WriteAllText(itemFilePath, json);
        }

        public static List<Item> GetItems()
        {
            string itemFilePath = Utils.GetItemsFilePath();

            if (!File.Exists(itemFilePath))
            {
                return new List<Item>();
            }
            var json = File.ReadAllText(itemFilePath);
            var items = JsonSerializer.Deserialize<List<Item>>(json);
            return items;
        }

        public static List<Item> CreateItem(string iName, float iPrice, Types iType)
        {
            List<Item> items = GetItems();
            bool itemExist = items.Any(x => x.itemName == iName);

            if (itemExist)
            {
                throw new Exception($"{iName} already exists");
            }
            items.Add(
                new Item
                {
                    itemName = iName,
                    itemPrice = iPrice,
                    itemType = iType
                }
                );

            SaveItem(items);
            return items; //mathi save gareko items bolako
        }

        public static List<Item> ChangePrice(string iName, float iPrice)
        {
            List<Item> items = GetItems();

            Item item = items.FirstOrDefault(x => x.itemName == iName);

            if(item == null)
            {
                throw new Exception("Item Not Found!");
            }
            item.itemPrice = iPrice;
            SaveItem(items);
            return items;
        }

        public static List<Item> DeleteItem(string iName)
        {
            List<Item> items = GetItems();
            Item item = items.FirstOrDefault( x => x.itemName == iName);

            if(item == null)
            {
                throw new Exception("Item Not Found!");
            }
            items.Remove(item);
            SaveItem(items);
            return items;
        }
    }
}