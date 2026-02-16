namespace YapayZekaSigorta.Entities
{
    public class PricingPlan
    {
        public int PricingPlanId { get; set; }
        public string? Title { get; set; }
        public string? Description1 { get; set; }
        public string? Description2 { get; set; }
        public string PlanName { get; set; }
        public decimal Price { get; set; }
        public bool IsFeature { get; set; }//plan ana sayfada gözükecek mi

        public List<PricingPlanItem> PricingPlanItems { get; set; }
    }
}
