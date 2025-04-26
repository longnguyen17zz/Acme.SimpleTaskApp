using Abp.Domain.Repositories;
using Abp.UI;
using Acme.SimpleTaskApp.Entities.CartItems;
using Acme.SimpleTaskApp.Entities.Carts;
using Acme.SimpleTaskApp.Entities.OrderItems;
using Acme.SimpleTaskApp.Entities.Orders;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Orders
{
    public class OrderAppService : SimpleTaskAppAppServiceBase, IOrderAppService
    {
        private readonly IRepository<Cart, Guid> _cartRepository;
        private readonly IRepository<CartItem, Guid> _cartItemRepository;
        private readonly IRepository<Order, Guid> _orderRepository;
        private readonly IRepository<OrderItem, Guid> _orderItemRepository;

        public OrderAppService(
            IRepository<Cart, Guid> cartRepository,
            IRepository<CartItem, Guid> cartItemRepository,
            IRepository<Order, Guid> orderRepository,
            IRepository<OrderItem, Guid> orderItemRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public async Task CreateOrderFromCartAsync(long userId, string shippingName, string shippingAddress, string shippingPhone)
        {
            if (userId == 0)
            {
                throw new Exception("Người dùng chưa đăng nhập.");
            }

            var cart = await _cartRepository.FirstOrDefaultAsync(x => x.UserId == userId);
            if (cart == null)
            {
                throw new UserFriendlyException("Không tìm thấy giỏ hàng.");
            }

            var cartItems = await _cartItemRepository.GetAllListAsync(x => x.CartId == cart.Id);
            if (!cartItems.Any())
            {
                throw new UserFriendlyException("Giỏ hàng trống.");
            }

            var totalAmount = cartItems.Sum(x => x.Price * x.Quantity);



            var order = new Order(userId,totalAmount, false, "Thanh toán khi nhận hàng", "Đang xử lý", shippingName, shippingAddress, shippingPhone)
            {
                UserId = userId,
                OrderDate = DateTime.Now
            };
            await _orderRepository.InsertAsync(order);

            // Chuyển CartItem → OrderItem
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem(item.Quantity, item.Price)
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId
                };

                await _orderItemRepository.InsertAsync(orderItem);
            }

            // Xóa giỏ hàng và cartItem
            await _cartItemRepository.DeleteAsync(x => x.CartId == cart.Id);
            await _cartRepository.DeleteAsync(cart);
        }
        public async Task GetAll()
        {
            await _orderRepository.GetAllAsync();
        }
    }


}
