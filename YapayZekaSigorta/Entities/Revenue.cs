namespace YapayZekaSigorta.Entities
{
    public class Revenue
    {
        public int RevenueId { get; set; }
        public string NameSurname { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime ProcessDate { get; set; }
    }
}
