using Acme.SimpleTaskApp.Carts.Dto;
using Acme.SimpleTaskApp.Orders.Dto;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.Checkouts
{
    public class CheckoutViewModel
    {
        public List<CartItemDto> CartItems { get; set; }
        public ShippingInfoDto ShippingInfo { get; set; }
    }
}
