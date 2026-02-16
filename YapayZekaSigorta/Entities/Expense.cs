namespace YapayZekaSigorta.Entities
{
    public class Expense
    {
        public int ExpenseId { get; set; }
        public string NameSurname { get; set; }
        public string Detail { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessDate { get; set; }
    }
}
