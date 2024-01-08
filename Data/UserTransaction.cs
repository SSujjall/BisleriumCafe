namespace BisleriumCafe.Data
{
    internal class UserTransaction
    {
        public long UserID { get; set; }
        public DayOfWeek DayOfSale { get; set; }
        public int TransactionCount { get; set; }
        public DateTime DateOfSale { get; set; }
    }
}