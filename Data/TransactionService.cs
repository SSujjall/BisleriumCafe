using System.Text.Json;

namespace BisleriumCafe.Data
{
    internal class TransactionService
    {
        public static List<Transaction> GetTransaction()
        {
            string itemFilePath = Utils.GetOrdersPath();

            if (!File.Exists(itemFilePath))
            {
                return new List<Transaction>();
            }

            var json = File.ReadAllText(itemFilePath);
            var transaction = JsonSerializer.Deserialize<List<Transaction>>(json);
            return transaction;
        }

        public static void SaveTransaction(List<Transaction> transaction)
        {
            string itemFilePath = Utils.GetOrdersPath();

            var json = JsonSerializer.Serialize(transaction);
            File.WriteAllText(itemFilePath, json);
        }

        public static List<Transaction> CreateTransaction (Transaction transactionData)
        {
            List<Transaction> transaction = GetTransaction();
            bool itemExist = transaction.Any(x => x.TransactionID == transactionData.TransactionID);

            if(DateTime.Now.DayOfWeek != DayOfWeek.Saturday)
            {
                transaction.Add(
                    new Transaction()
                    {
                        TransactionID = new Guid(),
                        Day = transactionData.Day,
                        SalesDate = DateTime.Now,
                        ItemName = transactionData.ItemName,
                        UserID = transactionData.UserID,
                    });
            }
            SaveTransaction(transaction);
            return transaction;
        }

        public static List<UserTransaction> GetTransactionByUserID (long userId)
        {
            string itemFilePath = Utils.GetOrdersPath();

            if (!File.Exists(itemFilePath))
            {
                return new List<UserTransaction>();
            }

            var json = File.ReadAllText(itemFilePath);
            var transactions = JsonSerializer.Deserialize<List<Transaction>>(json);

            var userTransactionData = transactions.Where(x => x.UserID == userId && x.SalesDate.Month == DateTime.Now.Month);

            var dataCount = userTransactionData.Count();
            var data = new List<UserTransaction>();

            foreach (var transaction in userTransactionData)
            {
                data.Add(
                    new UserTransaction()
                    {
                        UserID = transaction.UserID,
                        DateOfSale = transaction.SalesDate,
                        DayOfSale = transaction.Day,
                        TransactionCount = dataCount
                    });
            }
            return data;
        }
    }
}