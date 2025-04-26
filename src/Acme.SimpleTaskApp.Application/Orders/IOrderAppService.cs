using Abp.Application.Services;

using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Orders
{
    public interface IOrderAppService : IApplicationService
    {
        Task CreateOrderFromCartAsync(long userId, string shippingName, string shippingAddress, string shippingPhone);
    }
}
