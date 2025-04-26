using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Acme.SimpleTaskApp.CartItems.Dto;
using Acme.SimpleTaskApp.Carts.Dto;
using System;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Carts
{
    public interface ICartItemAppService : IApplicationService
    {

        public Task<ListResultDto<CartItemDto>> GetAll();

        public Task CreateAsync(AddCartItemDto input);

        public Task DeleteAsync(Guid cartItemId);

        public Task<int> GetCartCount();

        //public Task UpdateQuantityAsync(Guid cartItemId, int quantity);

    }
}
