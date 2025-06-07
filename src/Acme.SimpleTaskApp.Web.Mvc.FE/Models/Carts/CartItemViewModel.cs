using System;
namespace Acme.SimpleTaskApp.Web.Models.Carts
{
    public class CartItemViewModel
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public string Images { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }



        //public decimal Total => Price * Quantity;

        public CartItemViewModel(int cartItemId, int productId, string productName, string images, int quantity, decimal price)
        {
            CartItemId = cartItemId;
            ProductId = productId;
            ProductName = productName;
            Images = images;
            Quantity = quantity;
            Price = price;

        }

   

    }
}
