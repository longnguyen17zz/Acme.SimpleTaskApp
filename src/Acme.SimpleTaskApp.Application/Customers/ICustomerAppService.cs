using Abp.Application.Services;
using Acme.SimpleTaskApp.Customers.Dto;

using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Customers
{
    public interface ICustomerAppService : IApplicationService
    {
        Task CreateCustomerAsync(CreateCustomerDto input);
    }
}
