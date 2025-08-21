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
using Acme.SimpleTaskApp.Entities.CartItems;
using NUglify.Helpers;
using Acme.SimpleTaskApp.Carts.Dto;
using Acme.SimpleTaskApp.Products;
using Acme.SimpleTaskApp.VnPays;
using Acme.SimpleTaskApp.VnPays.Dto;

namespace Acme.SimpleTaskApp.Web.Controllers
{
    public class CheckoutsController : SimpleTaskAppControllerBase
    {
        private readonly UserManager _userManager;
        public readonly IOrderAppService _orderAppService;
        public readonly ICartItemAppService _cartItemAppService;
        public readonly IProductAppService _productAppService;

		    private readonly IVnPayService _vnPayService;


		public CheckoutsController(UserManager userManager, IOrderAppService orderAppService, ICartItemAppService cartItemAppService, IProductAppService productAppService, IVnPayService vnPayService)
        {
            _userManager = userManager;
             _orderAppService = orderAppService;
            _cartItemAppService = cartItemAppService;
            _productAppService = productAppService;
			      _vnPayService = vnPayService;
		}

        public async Task<IActionResult> Index(List<int> cartItemId, List<int> quantity, int? productId, int? buyNowQuantity)
        {
            if (productId.HasValue && buyNowQuantity.HasValue)
            {
                // Người dùng bấm "Mua ngay"
                var product = await _productAppService.GetByIdProduct(productId.Value); // Lấy thông tin sản phẩm

                if (product == null)
                {
                    return RedirectToAction("Index", "Home");
                }

                var cartItems = new List<CartItemDto>
                {
                    new CartItemDto
                    {
                        ProductId = productId.Value,
                        Quantity = buyNowQuantity.Value,
                        Price = product.Price,
                        ProductName = product.Name,
                        Images = product.Images
                    }
                };

                var shippingInfo = new ShippingInfoDto
                {
                    FullName = "", // Có thể lấy từ user
                    Address = "",
                    PhoneNumber = "",
                    PaymentMethod = "COD",
                };

                var model = new CheckoutViewModel
                {
                    CartItems = cartItems,
                    ShippingInfo = shippingInfo,
                    IsBuyNow = false,
                };

                return View(model); // Hiển thị sản phẩm mua ngay
            }

            // Nếu không phải "Mua ngay", thì xử lý giỏ hàng như bình thường
            for (int i = 0; i < cartItemId.Count; i++)
            {
                await _cartItemAppService.UpdateQuantityAsync(cartItemId[i], quantity[i]);
            }
            var cartItemsResult = await _cartItemAppService.GetAll();
            var newcartItems = cartItemsResult.Items.ToList();
            var shippingInfoDefault = new ShippingInfoDto
            {
                FullName = "", // có thể lấy từ user
                Address = "",
                PhoneNumber = "",
                PaymentMethod = "COD",
            };
            var defaultModel = new CheckoutViewModel
            {
                CartItems = newcartItems,
                ShippingInfo = shippingInfoDefault
            };

            return View(defaultModel);
        }

        public async Task<IActionResult> BuyNow(int productId, int buyNowQuantity, decimal? FlashSalePrice)
        {
            var product = await _productAppService.GetByIdProduct(productId);
            if (product == null)
            {
                return RedirectToAction("Index", "Home");  // Sản phẩm không tồn tại
            }
            var discountPrice = product.Price - FlashSalePrice;
            var finalPrice = discountPrice ?? product.Price;

            // Tạo CheckoutViewModel cho việc thanh toán "Mua ngay"
            var model = new CheckoutViewModel
            {
                ProductName = product.Name,
                Images = product.Images,  // Đảm bảo bạn có trường Image trong sản phẩm
                Price = finalPrice,
                Quantity = buyNowQuantity,
                ProductId= product.Id,
                IsBuyNow = true,
                ShippingInfo = new ShippingInfoDto()  // Thông tin giao hàng ban đầu trống
            };

            return View("Index", model);  
        } 
        [HttpPost]
        public async Task<IActionResult> Checkout(string shippingName, string shippingAddress, string ward, string province, string shippingPhone, int? productId, int? buyNowQuantity)
        {
            var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
            if (user == null)
            {
                throw new Exception("Người dùng không tồn tại.");
            }
            var userId = user.Id;
            int orderId;

            if (productId.HasValue && buyNowQuantity.HasValue)
            {
                // Mua ngay
                orderId = await _orderAppService.CreateOrderDirectAsync(userId, productId.Value, buyNowQuantity.Value, shippingName, shippingAddress, ward, province, shippingPhone);
            }
            else
            {
                // Giỏ hàng
                orderId = await _orderAppService.CreateOrderFromCartAsync(userId, shippingName, shippingAddress, ward, province, shippingPhone);
            }

            return RedirectToAction("OrderSuccess", "Orders", new { id = orderId });
        }

		    public IActionResult CreatePaymentUrl(PaymentInformationDto model)
		    {
			    var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

			    return Redirect(url);
		    }

		    public IActionResult PaymentCallback()
		    {
			    var response = _vnPayService.PaymentExecute(Request.Query);

			    return Json(response);
		    }

	}
}
