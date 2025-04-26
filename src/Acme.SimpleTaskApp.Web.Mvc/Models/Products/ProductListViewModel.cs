using Acme.SimpleTaskApp.Categories.Dto;
using Acme.SimpleTaskApp.Products.Dto;
using System.Collections.Generic;
namespace Acme.SimpleTaskApp.Web.Models.Products;

    public class ProductListViewModel
    {
        public IReadOnlyList<ProductDto> Products { get; }

    

        public ProductListViewModel(IReadOnlyList<ProductDto> products)
            {
                Products = products;
            
        }
    }

