using Acme.SimpleTaskApp.Products.Dto;

namespace Acme.SimpleTaskApp.Web.Models.HomepageProducts
{
    public class ProductDetailViewModel
    {
        public ProductDto Product { get; set; }


        public ProductDetailViewModel(ProductDto product)
        {
            Product = product;
        }
    }
}
