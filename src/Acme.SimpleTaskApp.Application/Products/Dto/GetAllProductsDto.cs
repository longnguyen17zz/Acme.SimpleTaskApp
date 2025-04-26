using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Entities.Products;
using System;


namespace Acme.SimpleTaskApp.Products.Dto;
public class GetAllProductsDto : ProductInput
{
    
}

[AutoMapFrom(typeof(Product))]
public class TaskListDto : EntityDto<Guid>, IHasCreationTime
{
   
    public string Name { get; set; }

    public string Description { get; set; }

    public string Images { get; set; }

    public decimal Price { get; set; }

    public int StockQuantity { get; set; }

    public string CategoryId { get; set; }

        public string CategoryName { get; set; }


    public DateTime CreationTime { get; set; }

}
