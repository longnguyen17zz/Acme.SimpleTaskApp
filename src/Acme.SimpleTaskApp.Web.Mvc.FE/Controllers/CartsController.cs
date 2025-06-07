using Abp.Domain.Repositories;
using Acme.SimpleTaskApp.Authorization.Users;
using Acme.SimpleTaskApp.CartItems.Dto;
using Acme.SimpleTaskApp.Carts;
using Acme.SimpleTaskApp.Controllers;
using Acme.SimpleTaskApp.Entities.CartItems;
using Acme.SimpleTaskApp.Orders;
using Acme.SimpleTaskApp.Web.Models.Carts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Web.Controllers
{
    public class CartsController : SimpleTaskAppControllerBase
    {
        public readonly ICartItemAppService _cartItemAppService;
        public readonly IOrderAppService _orderAppService;
        public CartsController(ICartItemAppService cartItemAppService, IOrderAppService orderAppService, IRepository<CartItem> cartItemRepository)
        {
            _cartItemAppService = cartItemAppService;
            _orderAppService = orderAppService;
        }
        //fixx
        public async Task<ActionResult> Index()
        {
            var carts = await _cartItemAppService.GetAll();
           
            var model = new CartViewModel(
               carts.Items.Select(x => new CartItemViewModel(
                x.CartItemId,
               x.ProductId,
               x.ProductName ?? "Sản phẩm",
               x.Images ?? "/images/no-image.jpg",
               x.Quantity,
               x.Price
             )).ToList());
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity, decimal? unitPrice)
        {

             var cartItem = new AddCartItemDto
            {
                ProductId = productId,
                Quantity = quantity,
                FlashSalePrice = unitPrice,
            };
             await _cartItemAppService.CreateAsync(cartItem);

            return Ok(); // báo thành công cho AJAX
        }
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            await _cartItemAppService.DeleteAsync(cartItemId);

            var results = await _cartItemAppService.GetAll();
            var model = new CartViewModel(
              results.Items.Select(x => new CartItemViewModel(
               x.CartItemId,
              x.ProductId,
              x.ProductName ?? "Sản phẩm",
              x.Images ?? "/images/no-image.jpg",
              x.Quantity,
              x.Price
            )).ToList());

            return PartialView("_CartItemsPartial", model);
        }
        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var userId = AbpSession.UserId;
            if (userId == null)
                return Content("0");
            var count = await _cartItemAppService.GetCartCount();
            return Content(count.ToString());
        }

        [HttpPost]
        public IActionResult DeleteSelected(List<int> selectedIds)
        {
            if (selectedIds == null || !selectedIds.Any())
            {
                TempData["Error"] = "Vui lòng chọn sản phẩm để xoá.";
                return RedirectToAction("Index");
            }

            foreach (var id in selectedIds)
            {
                // Gọi service để xóa từng sản phẩm theo ID
                _cartItemAppService.DeleteItem(id);
            }

            TempData["Success"] = "Đã xoá các sản phẩm đã chọn.";
            return RedirectToAction("Index");
        }


    }
}
