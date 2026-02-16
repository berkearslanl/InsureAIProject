namespace YapayZekaSigorta.Models
{
    public class RevenueExpenseChartViewModel
    {
        public List<string> Months { get; set; }
        public List<decimal> revenueTotals { get; set; }
        public List<decimal> expenseTotals { get; set; }
    }
}
