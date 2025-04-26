using Acme.SimpleTaskApp.Categories.Dto;
using Acme.SimpleTaskApp.Products.Dto;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.Products
{
    public class EditProductViewModel
    {
        public ProductDto Product { get; set; }

        public IReadOnlyList<CategoryDto>  Categories { get; set; }

        public string CategoryId
        {
            get => Product.CategoryId;
            set
            {
                if (Product != null)
                {
                    Product.CategoryId = value;
                }
            }
        }
    }
}
