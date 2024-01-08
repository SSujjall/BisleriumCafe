namespace BisleriumCafe.Data
{
    internal class Transaction
    {
        public Guid TransactionID { get; set; }
        public DateTime SalesDate { get; set; }
        public long UserID { get; set; }
        public DayOfWeek Day { get; set; }
        public string ItemName { get; set; }

    }
}
