using Abp.Domain.Repositories;
using Acme.SimpleTaskApp.Authorization.Users;
using Acme.SimpleTaskApp.DashBoards.Dto;
using Acme.SimpleTaskApp.Entities.Orders;
using Acme.SimpleTaskApp.Entities.Products;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.DashBoards
{
    public class DashBoardAppService : SimpleTaskAppAppServiceBase, IDashBoardAppService
    {

        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<User,long> _userRepository;


        public DashBoardAppService(IRepository<Order> orderRepository, IRepository<Product> productRepository, IRepository<User, long> userRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<DashBoardDto> GetDashboardDataAsync()
        {
            var orders = await _orderRepository.GetAll()
                .Where(o => o.OrderDate.Year == DateTime.Now.Year)
                .ToListAsync();

            var totalRevenue = orders
                .Where(o => o.Status == "Hoàn thành")
                .Sum(o => o.TotalAmount);

            var revenuePerMonth = orders
                .Where(o => o.Status == "Hoàn thành")
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new ChartDataDto
                {
                    Label = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key),
                    Value = g.Sum(x => x.TotalAmount)
                }).ToList();
            var ordersPerMonth = orders
                .Where(o => o.Status == "Hoàn thành")
                .GroupBy(o => o.OrderDate.Month)
                .Select(g => new ChartDataDto
                {
                    Label = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(g.Key),
                    Value = g.Count()
                }).ToList();
            var statusSummary = new OrderStatusSummaryDto
            {
                Pending = orders.Count(x => x.Status == "Đang xử lý"),
                Cancelled = orders.Count(x => x.Status == "Đã hủy"),
                Completed = orders.Count(x => x.Status == "Hoàn thành")
            };

            var totalCustomers = await _userRepository.GetAll()
                .Where(u => u.TenantId != null)
                .CountAsync();

            return new DashBoardDto
            {
                TotalRevenue = totalRevenue,
                TotalOrders = orders.Count,
                TotalCustomers = totalCustomers,
                RevenuePerMonth = revenuePerMonth,
                OrdersPerMonth = ordersPerMonth,
                OrderStatusSummary = statusSummary
            };
        }
    }
}
