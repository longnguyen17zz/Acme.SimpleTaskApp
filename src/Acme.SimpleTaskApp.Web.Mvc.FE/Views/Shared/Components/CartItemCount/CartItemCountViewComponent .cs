using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Acme.SimpleTaskApp.Entities.CartItems;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using Abp.AspNetCore.Mvc.ViewComponents;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;

namespace Acme.SimpleTaskApp.Web.Views.Shared.Components.CartItemCount
{
    public class CartItemCountViewComponent : SimpleTaskAppViewComponent
    {
        private readonly IRepository<CartItem> _cartItemRepository;
        private readonly IAbpSession _abpSession;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public CartItemCountViewComponent(
            IRepository<CartItem> cartItemRepository,
            IAbpSession abpSession,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cartItemRepository = cartItemRepository;
            _abpSession = abpSession;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!_abpSession.UserId.HasValue)
            {
                return View(0);
            }

            using (var uow = _unitOfWorkManager.Begin())
            {
                var count = await _cartItemRepository
                    .GetAll()
                    .Where(x => x.Cart.UserId == _abpSession.UserId.Value)
                    .CountAsync();

                await uow.CompleteAsync();
                return View(count);
            }
        }
    }

}
