using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Acme.SimpleTaskApp.Categories.Dto;

using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Categories
{
    public interface ICategoryAppService : IApplicationService
    
    {
        Task<ListResultDto<CategoryDto>> GetAllAsync();

        Task<CategoryDto> GetByIdAsync(EntityDto<string> input);
        Task CreateAsync(CategoryDto input);


        Task UpdateAsync(CategoryDto input);

        Task DeleteAsync(EntityDto<string> input);

        Task<PagedResultDto<CategoryDto>> GetPagedAsync(CategorytInput input);
    }
}
