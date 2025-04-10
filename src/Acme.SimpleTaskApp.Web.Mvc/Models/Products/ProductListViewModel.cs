using Acme.SimpleTaskApp.Products.Dto;
using System.Collections.Generic;
namespace Acme.SimpleTaskApp.Web.Models.Products;

    public class ProductListViewModel
    {
        public IReadOnlyList<TaskListDto> Products { get; }

        public ProductListViewModel(IReadOnlyList<TaskListDto> products)
        {
            Products = products;
        }
    }

