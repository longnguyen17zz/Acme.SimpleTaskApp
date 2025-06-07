using System;
using System.Collections.Generic;

using static Acme.SimpleTaskApp.Orders.Dto.GetOrderDetailsOutput;

namespace Acme.SimpleTaskApp.Orders.Dto
{
    public class OrderDto
    {
        public DateTime OrderDate { get; set; }           // Ngày đặt
        public string PaymentMethod { get; set; }         // Phương thức
        public decimal TotalAmount { get; set; }          // Tổng tiền
        public string Status { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public string FullName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
