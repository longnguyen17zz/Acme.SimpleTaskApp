
using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using Acme.SimpleTaskApp.Products.Dto;
using System;
namespace Acme.SimpleTaskApp.Products
{
    public interface IProductAppService : IApplicationService
    {
        Task<ListResultDto<ProductDto>> GetAll();
        System.Threading.Tasks.Task Create(CreateProductDto input);
        Task<ProductDto> GetByIdAsync(EntityDto<Guid> input);

        Task UpdateProductData(UpdateProductDto input);
        Task DeleteAsync(EntityDto<Guid> input);

        Task<PagedResultDto<ProductDto>> GetPagedAsync(ProductInput input);

        Task<PagedResultDto<ProductDto>> GetPagedForUserAsync(ProductInputUser input);


    }
}
