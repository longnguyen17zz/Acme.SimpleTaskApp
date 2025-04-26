using Acme.SimpleTaskApp.Categories.Dto;
using Acme.SimpleTaskApp.Products.Dto;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.HomepageProducts
{
    public class HomepageViewModel
    {
        public IReadOnlyList<CategoryDto> Categories { get; set; }
        public IReadOnlyList<ProductDto> Products { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public string Keyword { get; set; }
        public string CategoryId { get; set; }

        public string CurrentCategoryId { get; set; }

        public HomepageViewModel(IReadOnlyList<CategoryDto> categories, IReadOnlyList<ProductDto> products,int currentPage,int pageSize,int totalPages,string keyword,string categoryId,string currentCategoryId) 
        { 
            Categories = categories;
            Products = products;
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            Keyword = keyword;
            CategoryId = categoryId;
            CurrentCategoryId = currentCategoryId;
        }
    }
}
