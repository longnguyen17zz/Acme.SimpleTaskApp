using System.Collections.Generic;
using System;

namespace Acme.SimpleTaskApp.Web.Models.InfoOrder
{
    public class InfoOrderDetailViewModel
    {
        public int OrderId { get; set; }
        public string PaymentMethod { get; set; }         // Phương thức
        public decimal TotalAmount { get; set; }          // Tổng tiền
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }

        public List<OrderDetailUserDto> Items { get; set; }

        public class OrderDetailUserDto
        {
            public string Image { get; set; }
            public string ProductName { get; set; }

            public int Quantity { get; set; }

            public decimal Price { get; set; }


        }
    }
}
