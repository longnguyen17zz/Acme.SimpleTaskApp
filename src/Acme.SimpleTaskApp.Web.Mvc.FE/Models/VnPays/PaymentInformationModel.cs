namespace Acme.SimpleTaskApp.Web.Models.VnPays
{
	public class PaymentInformationModel
	{
		public string OrderType { get; set; }
		public double Amount { get; set; }
		public string OrderDescription { get; set; }
		public string Name { get; set; }
	}
}
