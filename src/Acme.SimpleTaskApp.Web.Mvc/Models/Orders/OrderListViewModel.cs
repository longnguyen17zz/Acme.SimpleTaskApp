using Acme.SimpleTaskApp.Orders.Dto;
using Acme.SimpleTaskApp.Products.Dto;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.Orders
{
    public class OrderListViewModel
    {
        public IReadOnlyList<ShippingInfoDto> Orders { get; }



        public OrderListViewModel(IReadOnlyList<ShippingInfoDto> orders)
        {
            Orders = orders;
        }

    }
}
