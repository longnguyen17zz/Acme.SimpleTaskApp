using Acme.SimpleTaskApp.Orders.Dto;
using System;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.InfoOrder
{
    public class InfoOrderListViewModel
    {
        public List<GetOrderDetailsOutput> OrderItems { get; set; }
    }
}
