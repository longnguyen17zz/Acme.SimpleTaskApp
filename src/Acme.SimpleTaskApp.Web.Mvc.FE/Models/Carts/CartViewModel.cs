using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.Carts
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new List<CartItemViewModel>();
        public CartViewModel(List<CartItemViewModel> items)
        {
            Items = items;
        }
    }
}
