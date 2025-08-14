using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Entities.Orders;
using Acme.SimpleTaskApp.Entities.Products;
using System;
using System.ComponentModel.DataAnnotations.Schema;


namespace Acme.SimpleTaskApp.Entities.OrderItems
{
    [Table("AppOrderItems")]

    public class OrderItem : FullAuditedEntity<int>
	{

        public int OrderId { get; set; }

        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        public int  ProductId   { get; set; }
        
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public decimal UnitPrice    { get; set; }

        public decimal Total => Quantity * UnitPrice;
        public OrderItem(int quantity, decimal unitPrice)
        {
            
            Quantity = quantity;
            UnitPrice = unitPrice;
        }


        public OrderItem(int quantity)
        {
            Quantity = quantity;
        }
    }
}
