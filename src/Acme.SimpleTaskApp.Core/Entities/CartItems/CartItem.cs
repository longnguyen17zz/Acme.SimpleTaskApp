using Abp.Domain.Entities;
using Acme.SimpleTaskApp.Entities.Carts;
using Acme.SimpleTaskApp.Entities.Orders;
using Acme.SimpleTaskApp.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Entities.CartItems
{
    [Table("AppCartItems")]

    public class CartItem : Entity<int>
    {
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        [NotMapped]
        public decimal Total => Price * Quantity;

        public DateTime CreationTime { get; set; }
        public string ProductName { get; set; }

        public CartItem()
        {
            CreationTime = DateTime.Now;
            
        }
        public CartItem(int quantity, decimal price)
            :this()
        {
            Quantity = quantity;
            Price = price;
        }
    }
}
