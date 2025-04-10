using Abp.Application.Services;
using Acme.SimpleTaskApp.Sessions.Dto;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Sessions;

public interface ISessionAppService : IApplicationService
{
    Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
}
