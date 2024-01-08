namespace BisleriumCafe.Data
{
    internal class UserTransaction
    {
        public long UserId { get; set; }
        public DayOfWeek SalesDay { get; set; }
        public int TransactionCount { get; set; }
        public DateTime SaleDate { get; set; }
    }
}