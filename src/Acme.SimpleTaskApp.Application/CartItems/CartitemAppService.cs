using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.UI;
using Acme.SimpleTaskApp.CartItems.Dto;
using Acme.SimpleTaskApp.Carts.Dto;
using Acme.SimpleTaskApp.Entities.CartItems;
using Acme.SimpleTaskApp.Entities.Carts;
using Acme.SimpleTaskApp.Entities.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Carts
{
    public class CartItemAppService : SimpleTaskAppAppServiceBase, ICartItemAppService
    {
        private readonly IRepository<CartItem> _cartItemRepository;
        private readonly IRepository<Cart> _cartRepository;
        private readonly IRepository<Product> _productRepository;

        public CartItemAppService(IRepository<CartItem> cartItemRepository, IRepository<Cart> cartRepository, IRepository<Product> productRepository)
        {
            _cartItemRepository = cartItemRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }
        public async Task<ListResultDto<CartItemDto>> GetAll()
        {
            var userId = AbpSession.UserId.Value;

            var cart = await _cartRepository.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                return new ListResultDto<CartItemDto>();
            }
            var cartItems = await _cartItemRepository
                .GetAll()
                .Where(x => x.CartId == cart.Id)
                .Include(x => x.Product)
                .ToListAsync();

            var result = cartItems.Select(item => new CartItemDto
            {
                CartItemId = item.Id,
                ProductName = item.Product != null ? item.Product.Name : "Sản phẩm không tồn tại",
                Quantity = item.Quantity,
                Price = item.Price,
                Images = item.Product != null ? item.Product.Images : "không có ảnh"
            }).ToList();

            return new ListResultDto<CartItemDto>(result);
        }

        public async Task CreateAsync(AddCartItemDto input)
        {
            var userId = AbpSession.UserId.Value;

            var cart = await _cartRepository.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
            {
                cart = new Cart(userId);
                await _cartRepository.InsertAsync(cart);
                await CurrentUnitOfWork.SaveChangesAsync();
            }
            var product = await _productRepository.GetAsync(input.ProductId);

            var discountFlash = product.Price - input.FlashSalePrice;
            var finalPrice = discountFlash ?? product.Price;

            var existingItem = _cartItemRepository
                .GetAll()
                .FirstOrDefault(x => x.CartId == cart.Id && x.ProductName == product.Name);

            if (existingItem != null)
            {
                existingItem.Quantity += input.Quantity;
                await _cartItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = input.ProductId,
                    ProductName = product.Name,
                    Quantity = input.Quantity,
                    Price = finalPrice
                };

                await _cartItemRepository.InsertAsync(cartItem);
            }
        }
        public async Task DeleteAsync(int cartItemId)
        {
            await _cartItemRepository.DeleteAsync(cartItemId);
        }

        public async Task<int> GetCartCount()
        {
            var userId = AbpSession.UserId;
            var cartCount = _cartItemRepository
                .GetAll()
                .Where(x => x.Cart.UserId == userId.Value)
                .Count();
            return cartCount;
        }


        public async Task UpdateQuantityAsync(int cartItemId, int quantity)
        {
            var cartItem = await _cartItemRepository.GetAsync(cartItemId);
            if (cartItem == null)
            {
                throw new UserFriendlyException("Sản phẩm trong giỏ hàng không tồn tại.");
            }

            cartItem.Quantity = quantity;
            await _cartItemRepository.UpdateAsync(cartItem);
        }

        public void DeleteItem(int cartItemId)
        {
            var cartItem = _cartItemRepository.FirstOrDefault(cartItemId);
            if (cartItem == null)
            {
                throw new UserFriendlyException("Không tìm thấy sản phẩm trong giỏ hàng.");
            }

            _cartItemRepository.Delete(cartItem);
        }
    }
}
