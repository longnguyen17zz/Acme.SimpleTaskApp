using Acme.SimpleTaskApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using Acme.SimpleTaskApp.Authorization.Users;
using Acme.SimpleTaskApp.Orders;
using Acme.SimpleTaskApp.Web.Models.Checkouts;
using Acme.SimpleTaskApp.Carts;
using System.Linq;
using Acme.SimpleTaskApp.Orders.Dto;
using Acme.SimpleTaskApp.CartItems.Dto;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Controllers
{
    public class CheckoutsController : SimpleTaskAppControllerBase
    {
        private readonly UserManager _userManager;
        public readonly IOrderAppService _orderAppService;
        public readonly ICartItemAppService _cartItemAppService;

        public CheckoutsController(UserManager userManager, IOrderAppService orderAppService, ICartItemAppService cartItemAppService)
        {
            _userManager = userManager;
             _orderAppService = orderAppService;
            _cartItemAppService = cartItemAppService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = AbpSession.UserId;

            var cartItemsResult = await _cartItemAppService.GetAll();
            var cartItems = cartItemsResult.Items.ToList();
            var totalAmount = cartItems.Sum(x => x.Price * x.Quantity);

            var shippingInfo = new ShippingInfoDto
            {
                FullName = "", // sau có thể lấy từ user
                Address = "",
                PhoneNumber = "",
                PaymentMethod = "COD",
                TotalAmount = totalAmount
            };
            var model = new CheckoutViewModel
            {
                CartItems = cartItems,
                ShippingInfo = shippingInfo
            };

            return View(model);
        }


        //[HttpPost]
        //public async Task<IActionResult> Index(List<CartItemInputDto> cartItems)
        //{
        //    foreach (var item in cartItems)
        //    {
        //        await _cartItemAppService.UpdateQuantityAsync(item.CartItemId, item.Quantity);
        //    }

        //    return RedirectToAction("Index"); // Tải lại trang thanh toán với số lượng mới
        //}


        [HttpPost]
        public async Task<IActionResult> Checkout(string shippingName, string shippingAddress, string shippingPhone)
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại.");
            }
            var userId = user.Id;
            // Cập nhật số lượng sản phẩm trong 
            await _orderAppService.CreateOrderFromCartAsync(userId, shippingName, shippingAddress, shippingPhone);

            return RedirectToAction("OrderSuccess", "Orders");
        }
    }
}
