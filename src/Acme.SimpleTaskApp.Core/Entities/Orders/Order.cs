using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Authorization.Users;
using Acme.SimpleTaskApp.Entities.OrderItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Acme.SimpleTaskApp.Entities.Orders
{
    [Table("AppOrders")]

    public class Order : FullAuditedEntity<Guid>
    {
        public long UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsPaid { get; set; }  
        public string PaymentMethod { get; set; }
        public string Status { get; set; }

        // Thông tin đặt hàng của khách
        public string ShippingName { get; set; }
        public string ShippingAddress { get; set; }
        public string ShippingPhone { get; set; }

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
        public Order(
            long userId,
            decimal totalAmount,
            bool isPaid,
            string paymentMethod,
            string status,
            string shippingName,
            string shippingAddress,
            string shippingPhone
        )
        {
            UserId = userId;
            TotalAmount = totalAmount;
            IsPaid = isPaid;
            PaymentMethod = paymentMethod;
            Status = status;
            ShippingName = shippingName;
            ShippingAddress = shippingAddress;
            ShippingPhone = shippingPhone;
            OrderDate = DateTime.Now;
        }
        public Order() { }
    }

}
