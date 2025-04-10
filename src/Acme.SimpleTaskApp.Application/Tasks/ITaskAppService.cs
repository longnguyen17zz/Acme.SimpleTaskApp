using Abp.Application.Services.Dto;
using Abp.Application.Services;
using System.Threading.Tasks;
using Acme.SimpleTaskApp.Tasks.Dto;

namespace Acme.SimpleTaskApp.Tasks
{
    public interface ITaskAppService : IApplicationService
    {
        Task<ListResultDto<TaskListDto>> GetAll(GetAllTasksInput input);
        System.Threading.Tasks.Task Create(CreateTaskInput input);
    }
}
