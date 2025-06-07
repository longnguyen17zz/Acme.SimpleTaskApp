using Acme.SimpleTaskApp.Carts.Dto;
using Acme.SimpleTaskApp.Orders.Dto;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.Checkouts
{
    public class CheckoutViewModel
    {
        public List<CartItemDto> CartItems { get; set; }
        public ShippingInfoDto ShippingInfo { get; set; }
        public string ProductName { get; set; }
        public string Images { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }

        public bool IsBuyNow { get; set; }

        public CheckoutViewModel()
        {
        }
        public CheckoutViewModel(List<CartItemDto> cartItems, ShippingInfoDto shippingInfo)
        {
            CartItems = cartItems;
            ShippingInfo = shippingInfo;
              
        }
        public CheckoutViewModel(string productName, string images, int quantity, decimal price, int productId)
        {
            ProductName = productName;
            Images = images;
            Quantity = quantity;
            Price = price;
            ProductId = productId;
        }
    }
}
