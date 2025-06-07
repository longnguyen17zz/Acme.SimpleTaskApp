namespace Acme.SimpleTaskApp.Web.Models.Dashboards
{
    public class TopSellingProductViewModel
    {
        public string Name { get; set; }
        public string Image  { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        public int TotalSold { get; set; }
    }
}
