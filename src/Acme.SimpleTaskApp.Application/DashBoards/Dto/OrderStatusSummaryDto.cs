
namespace Acme.SimpleTaskApp.DashBoards.Dto
{
    public class OrderStatusSummaryDto
    {
        public int Pending { get; set; }
        public int Cancelled { get; set; }

        public int Completed { get; set; }
    }
}
