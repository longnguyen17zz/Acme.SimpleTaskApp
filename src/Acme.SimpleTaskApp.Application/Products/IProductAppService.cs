
using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using Acme.SimpleTaskApp.Products.Dto;
using TaskListDto = Acme.SimpleTaskApp.Products.Dto.TaskListDto;
using Acme.SimpleTaskApp.Users.Dto;

namespace Acme.SimpleTaskApp.Products
{
    public interface IProductAppService : IApplicationService
    {
        Task<ListResultDto<TaskListDto>> GetAll(GetAllProductsDto input);
        System.Threading.Tasks.Task Create(CreateProductDto input);
        Task<ProductDto> GetAsync(EntityDto<int> input);

        Task UpdateProductData(UpdateProductDto input);
        Task DeleteAsync(EntityDto<int> input);
    }
}
