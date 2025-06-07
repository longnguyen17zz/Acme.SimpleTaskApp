using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Orders.Dto
{
    public class ShippingInfoDto : Entity<int>
    {
        
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public DateTime OrderDate { get; set; }

        public List<ProductItems> Items { get; set; }

        public class ProductItems
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
            public decimal UnitPrice { get; set; }
        }
    }
}
