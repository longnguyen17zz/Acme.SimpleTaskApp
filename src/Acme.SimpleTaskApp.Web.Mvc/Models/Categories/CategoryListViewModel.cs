using Acme.SimpleTaskApp.Categories.Dto;
using Acme.SimpleTaskApp.Products.Dto;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.Categories
{
    public class CategoryListViewModel
    {
        public IReadOnlyList<CategoryDto> Categories { get; }
        public CategoryListViewModel(IReadOnlyList<CategoryDto> categories)
        {
            Categories = categories;
        }

       
    }

    
}
