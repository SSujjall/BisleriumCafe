using System.Text.Json;

namespace BisleriumCafe.Data
{
    internal class OrderService
    {
        public static List<Order> GetOrderDetails()
        {
            string transactionFilePath = Utils.GetTransactionPath();

            if (!File.Exists(transactionFilePath))
            {
                return new List<Order>();
            }

            var json = File.ReadAllText(transactionFilePath);
            var transactions = JsonSerializer.Deserialize<List<Order>>(json);
            return transactions;
        }

        public static void SaveTransaction(List<Order> transactions)
        {
            string transactionFilePath = Utils.GetTransactionPath();

            var json = JsonSerializer.Serialize(transactions);
            File.WriteAllText(transactionFilePath, json);
        }

        public static List<Order> CreateNewOrder(Order transactionData)
        {
            List<Order> transactions = GetOrderDetails();
            bool itemExists = transactions.Any(x => x.TransactionID == transactionData.TransactionID);

            if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday)
            {
                transactions.Add(
                              new Order()
                              {
                                  TransactionID = new Guid(),
                                  Products = transactionData.Products,
                                  MemberID = transactionData.MemberID,
                                  DateOfSale = DateTime.Now,
                                  TotalPrice = transactionData.TotalPrice,
                              }
                          );
            }
            SaveTransaction(transactions);
            return transactions;
        }

        public static List<UserTransactionData> GetOrderByUserId(long userId)
        {
            string transactionFilePath = Utils.GetTransactionPath();

            if (!File.Exists(transactionFilePath))
            {
                return new List<UserTransactionData>();
            }

            var json = File.ReadAllText(transactionFilePath);
            var transactions = JsonSerializer.Deserialize<List<Order>>(json);

            var userTransactionData = transactions.Where(x => x.MemberID == userId && x.DateOfSale.Month == DateTime.Now.Month);

            var dataCount = userTransactionData.Count();
            var data = new List<UserTransactionData>();

            foreach (var transaction in userTransactionData)
            {
                data.Add(new UserTransactionData()
                {
                    UserId = transaction.MemberID,
                    SalesDate = transaction.DateOfSale,
                });
            }
            return data;
        }
    }
}
