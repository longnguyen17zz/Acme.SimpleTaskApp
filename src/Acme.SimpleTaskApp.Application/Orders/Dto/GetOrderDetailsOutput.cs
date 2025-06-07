using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Orders.Dto
{
    public class GetOrderDetailsOutput
    {
        public int OrderId { get; set; }
        public decimal TotalAmount { get; set; }          // Tổng tiền
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItemDto> Items { get; set; }

        public class OrderItemDto
        {
            public string Image { get; set; }
            public string ProductName { get; set; }

            public int Quantity { get; set; }

            public decimal Price { get; set; }

           
        }
    }
}
