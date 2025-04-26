using Acme.SimpleTaskApp.Products.Dto;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Areas.UserPortal.Models.HomepageProducts
{
    public class ProductListViewModel
    {

        public IReadOnlyList<ProductDto> Products { get; set; }



        public ProductListViewModel(IReadOnlyList<ProductDto> products)
        {
            Products = products;

        }
    }
}
