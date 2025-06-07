
using System.Collections.Generic;


namespace Acme.SimpleTaskApp.DashBoards.Dto
{
    public class DashBoardDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int TotalCustomers { get; set; }
        public List<ChartDataDto> RevenuePerMonth { get; set; }
        public List<ChartDataDto> OrdersPerMonth { get; set; }
        public OrderStatusSummaryDto OrderStatusSummary { get; set; }
    }
}
