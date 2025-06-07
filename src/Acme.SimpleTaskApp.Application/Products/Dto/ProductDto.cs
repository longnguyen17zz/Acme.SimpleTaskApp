using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Acme.SimpleTaskApp.Entities.Products;
using System;

namespace Acme.SimpleTaskApp.Products.Dto
{
    [AutoMapFrom(typeof(Product))]
    public class ProductDto : EntityDto<int>
    {
     
        public string Name { get; set; }

        public string Description { get; set; }

        public string Images { get; set; }

        public decimal Price { get; set; }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreationTime { get; set; }

        public int StockQuantity { get; set; }

    }
}
