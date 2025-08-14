using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Timing;
using Abp.UI;
using Acme.SimpleTaskApp.Authorization.Users;
using Acme.SimpleTaskApp.Common.Dto;
using Acme.SimpleTaskApp.Entities.CartItems;
using Acme.SimpleTaskApp.Entities.Carts;
using Acme.SimpleTaskApp.Entities.FlashSaleItems;
using Acme.SimpleTaskApp.Entities.OrderItems;
using Acme.SimpleTaskApp.Entities.Orders;
using Acme.SimpleTaskApp.Entities.Products;
using Acme.SimpleTaskApp.Entities.Stocks;
using Acme.SimpleTaskApp.Orders.Dto;
using Acme.SimpleTaskApp.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using static Acme.SimpleTaskApp.Orders.Dto.GetOrderDetailsOutput;
using static NuGet.Packaging.PackagingConstants;

namespace Acme.SimpleTaskApp.Orders
{
	public class OrderAppService : SimpleTaskAppAppServiceBase, IOrderAppService
	{
		private readonly IRepository<Cart> _cartRepository;
		private readonly IRepository<CartItem> _cartItemRepository;
		private readonly IRepository<Order> _orderRepository;
		private readonly IRepository<OrderItem> _orderItemRepository;
		private readonly IRepository<Product> _productRepository;
		private readonly IRepository<Stock> _stockRepository;
		private readonly IRepository<FlashSaleItem> _flashSaleItemRepository;
		private readonly UserManager _userManager;
		private readonly OrderExcelExporter _orderExcelExporter;



		public OrderAppService(
				IRepository<Cart> cartRepository,
				IRepository<CartItem> cartItemRepository,
				IRepository<Order> orderRepository,
				IRepository<OrderItem> orderItemRepository,
				IRepository<Product> productRepository,
				IRepository<Stock> stockRepository,
				IRepository<FlashSaleItem> flashSaleItemRepository,
				UserManager userManager,
				OrderExcelExporter orderExcelExporter)
		{
			_cartRepository = cartRepository;
			_cartItemRepository = cartItemRepository;
			_orderRepository = orderRepository;
			_orderItemRepository = orderItemRepository;
			_productRepository = productRepository;
			_stockRepository = stockRepository;
			_productRepository = productRepository;
			_flashSaleItemRepository = flashSaleItemRepository;
			_userManager = userManager;
			_orderExcelExporter = orderExcelExporter;
		}
		// Các hàm bên admin 
		public async Task<PagedResultDto<ShippingInfoDto>> GetPagedAsync(OrderInput input)
		{

			var query = _orderRepository
					.GetAll()
					.Include(p => p.Items)
					.AsQueryable();

			if (!string.IsNullOrWhiteSpace(input.Keyword))
			{
				query = query.Where(p => p.ShippingName.ToLower().Contains(input.Keyword.ToLower()));
			}
			if (!string.IsNullOrWhiteSpace(input.Status))
			{
				query = query.Where(p => p.Status == input.Status);
			}
			if (input.FromDate.HasValue)
			{
				query = query.Where(x => x.CreationTime >= input.FromDate.Value);
			}
			if (input.ToDate.HasValue)
			{
				query = query.Where(x => x.CreationTime <= input.ToDate.Value.AddDays(1));
			}
			var totalCount = await query.CountAsync();
			var orders = await query
					.OrderBy(input.Sorting)
					.PageBy(input)
					.ToListAsync();
			var result = orders.Select(p => new ShippingInfoDto
			{
				Id = p.Id,
				FullName = p.ShippingName,
				Address = p.ShippingAddress,
				PhoneNumber = p.ShippingPhone,
				PaymentMethod = p.PaymentMethod,
				Status = p.Status,
				OrderDate = p.OrderDate,
				TotalAmount = p.TotalAmount,
			}).ToList();
			return new PagedResultDto<ShippingInfoDto>(totalCount, result);
		}

		public async Task<OrderDetailDto> GetAsync(EntityDto<int> input)
		{
			var order = await _orderRepository
					.GetAllIncluding(o => o.Items) // nếu cần include
					.FirstOrDefaultAsync(o => o.Id == input.Id);

			if (order == null)
			{
				throw new UserFriendlyException("Đơn hàng không tồn tại.");
			}

			var dto = new OrderDetailDto
			{
				//Id = order.Id,
				OrderCode = order.Id,
				OrderDate = order.OrderDate,
				PaymentMethod = order.PaymentMethod,
				TotalAmount = order.TotalAmount,
				Status = order.Status,
			};
			return dto;
		}

		public async Task<OrderDto> GetOrderDetail(int orderId)
		{
			var orderDetail = await _orderRepository
					 .GetAll()
					 .Include(o => o.Items)
							 .ThenInclude(i => i.Product)
					 .FirstOrDefaultAsync(o => o.Id == orderId);
			var result = new OrderDto
			{
				PaymentMethod = orderDetail.PaymentMethod,
				TotalAmount = orderDetail.TotalAmount,
				Status = orderDetail.Status,
				OrderDate = orderDetail.OrderDate,
				FullName = orderDetail.ShippingName,
				Address = orderDetail.ShippingAddress,
				PhoneNumber = orderDetail.ShippingPhone,
				Items = orderDetail.Items.Select(i => new OrderItemDto
				{
					ProductName = i.Product.Name,
					Image = i.Product.Images,
					Quantity = i.Quantity,
					Price = i.UnitPrice
				}).ToList()

			};
			return result;
		}

		// Order Export
		public async Task<FileDto> ExportToExcel()
		{
			//var orders = await _orderRepository.GetAllIncluding(o => o.Items.Select(i => i.Product)).ToListAsync();
			var orders = await _orderRepository
			.GetAll()
			.Include(o => o.Items)
			.ThenInclude(i => i.Product)
			.ToListAsync();
			var orderDtos = orders.Select(o => new OrderDto
			{
				OrderDate = o.OrderDate,
				PaymentMethod = o.PaymentMethod,
				TotalAmount = o.TotalAmount,
				Status = o.Status,
				FullName = o.ShippingName,
				Address = o.ShippingAddress,
				PhoneNumber = o.ShippingPhone,
				Items = o.Items?.Select(i => new OrderItemDto
				{
					ProductName = i.Product.Name,
					Quantity = i.Quantity,
					Price = i.UnitPrice
				}).ToList()
			}).ToList();

			var fileBytes = await _orderExcelExporter.ExportToFileAsync(orderDtos);

			var fileName = "DanhSachDonHang.xlsx";
			return new FileDto(fileName, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
			{
				FileToken = Guid.NewGuid().ToString(), // Bạn có thể thay bằng file cache token nếu muốn lưu file vào server
				FileBytes = fileBytes
			};
		}

		// Các hàm bên user

		//Tạo order khi bấm mua ngay 
		public async Task<int> CreateOrderDirectAsync(long userId, int productId, int quantity, string shippingName, string shippingAddress, string ward, string district, string province, string shippingPhone)
		{
			// Kiểm tra người dùng
			if (userId <= 0)
			{
				throw new UserFriendlyException("Người dùng không hợp lệ.");
			}

			// Lấy sản phẩm
			var product = await _productRepository.FirstOrDefaultAsync(productId);
			if (product == null)
			{
				throw new UserFriendlyException("Sản phẩm không tồn tại.");
			}

			// Lấy tồn kho
			var stock = await _stockRepository.FirstOrDefaultAsync(s => s.ProductId == productId);
			if (stock == null || (stock.InitQuantity-stock.SellQuantity) < quantity)
			{
				throw new UserFriendlyException("Không đủ hàng trong kho.");
			}

			// Tính tổng tiền
			var totalPrice = product.Price * quantity;
			var address = $"{shippingAddress}, {ward}, {district}, {province}";
			// Tạo đơn hàng
			var order = new Order
			{
				UserId = userId,
				OrderDate = DateTime.Now,
				ShippingName = shippingName,
				ShippingAddress = address,
				ShippingPhone = shippingPhone,
				TotalAmount = totalPrice,
				PaymentMethod = "Thanh toán khi nhận hàng",
				Status = "Đang xử lý"
			};

			await _orderRepository.InsertAsync(order);
			await CurrentUnitOfWork.SaveChangesAsync(); // Đảm bảo order.Id được sinh ra

			// Tạo dòng chi tiết đơn hàng
			var orderItem = new OrderItem(quantity, product.Price)
			{
				OrderId = order.Id,
				ProductId = product.Id
			};

			await _orderItemRepository.InsertAsync(orderItem);

			// Trừ kho
			stock.SellQuantity += quantity;
			await _stockRepository.UpdateAsync(stock);

			return order.Id;
		}
		// Tạo order từ giỏ hàng
		public async Task<int> CreateOrderFromCartAsync(long userId, string shippingName, string shippingAddress, string ward, string district, string province, string shippingPhone)
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

			var address = $"{shippingAddress}, {ward}, {district}, {province}";

			var order = new Order(userId, totalAmount, false, "Thanh toán khi nhận hàng", "Đang xử lý", shippingName, address, shippingPhone)
			{
				UserId = userId,
				OrderDate = DateTime.Now
			};
			await _orderRepository.InsertAsync(order);
			await CurrentUnitOfWork.SaveChangesAsync();

			// Chuyển CartItem → OrderItem
			foreach (var item in cartItems)
			{
				var orderItem = new OrderItem(item.Quantity, item.Price)
				{
					OrderId = order.Id,
					ProductId = item.ProductId
				};
				await _orderItemRepository.InsertAsync(orderItem);

				var stock = await _stockRepository.FirstOrDefaultAsync(p => p.ProductId == item.ProductId);
				if (stock == null)
				{
					throw new UserFriendlyException($"Không tìm thấy sản phẩm với ID {item.ProductId}");
				}
				stock.SellQuantity += item.Quantity;
				await _stockRepository.UpdateAsync(stock);

				// Xử lý FlashSale trong khoảng thời gian và còn lượt mua thì có thể mua
				var flashSaleItem = await _flashSaleItemRepository
				.GetAll()
				.Include(x => x.FlashSale)
				.FirstOrDefaultAsync(x =>
						x.ProductId == item.ProductId &&
						x.FlashSale.StartTime <= Clock.Now &&
						x.FlashSale.EndTime >= Clock.Now);
				if (flashSaleItem != null)
				{
					// Kiểm tra còn lượt mua không
					if (flashSaleItem.Sold + item.Quantity > flashSaleItem.Quantity)
					{
						throw new UserFriendlyException($"Sản phẩm '{item.ProductId}' trong chương trình Flash Sale đã hết lượt mua.");
					}
					flashSaleItem.Sold += item.Quantity;
					await _flashSaleItemRepository.UpdateAsync(flashSaleItem);
				}
			}
			await _cartItemRepository.DeleteAsync(x => x.CartId == cart.Id);
			await _cartRepository.DeleteAsync(cart);
			return order.Id;
		}

		public async Task ConfirmDelivery(ConfirmDeliveryInput input)
		{
			var order = await _orderRepository.FirstOrDefaultAsync(input.OrderId);
			if (order == null)
			{
				throw new UserFriendlyException("Không tìm thấy đơn hàng");
			}

			if (order.Status == "Hoàn thành")
			{
				throw new UserFriendlyException("Đơn hàng đã được xác nhận trước đó.");
			}

			order.Status = "Hoàn thành";
			//order.CompletionTime = Clock.Now; // nếu bạn có trường này

			await _orderRepository.UpdateAsync(order);
		}


		// Dùng cả admin và user
		[HttpGet]
		public async Task<GetOrderDetailsOutput> GetOrderDetails(int orderId)
		{
			var items = await _orderItemRepository
				.GetAllIncluding(x => x.Product, x => x.Order)
				.Where(x => x.OrderId == orderId)
				.ToListAsync();

			if (items == null || items.Count == 0)
			{
				throw new UserFriendlyException("Không tìm thấy chi tiết đơn hàng.");
			}

			var result = items.Select(x => new GetOrderDetailsOutput.OrderItemDto
			{
				ProductName = x.Product.Name,
				Image = x.Product.Images,
				Quantity = x.Quantity,
				Price = x.UnitPrice,
			}).ToList();

			return new GetOrderDetailsOutput
			{
				OrderId = orderId,
				Items = result
			};
		}

		public async Task<List<TopSellingProductDto>> GetTopSellingProductsAsync()
		{
			var result = await _orderItemRepository
					.GetAll()
					.Include(od => od.Product)
							.ThenInclude(p => p.Category)
					.Include(od => od.Order)
					.Where(od => od.Order.Status == "Hoàn thành")
					.GroupBy(od => new
					{
						od.Product.Name,
						od.Product.Images,
						od.Product.Price,
						CategoryName = od.Product.Category.Name
					})
					.OrderByDescending(g => g.Sum(x => x.Quantity))
					.Select(g => new TopSellingProductDto
					{
						Name = g.Key.Name,
						Image = g.Key.Images,
						Price = g.Key.Price,
						CategoryName = g.Key.CategoryName,
						TotalSold = g.Sum(x => x.Quantity)
					})
					.Take(5)
					.ToListAsync();

			return result;
		}

		
		public async Task<List<GetOrderDetailsOutput>> GetOrderListUser()
		{
			var user = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
			if (user == null)
			{
				throw new Exception("Người dùng không tồn tại.");
			}
			var userId = user.Id;
			var orders = await _orderRepository
		 .GetAll()
		 .Include(o => o.Items)
		 .ThenInclude(oi => oi.Product)
		 .Where(o => o.UserId == user.Id)
		 .OrderByDescending(o => o.CreationTime)
		 .ToListAsync();

			var result = orders.Select(order => new GetOrderDetailsOutput
			{
				OrderId = order.Id,
				TotalAmount = order.TotalAmount,
				Status = order.Status,
				OrderDate = order.OrderDate,
				Items = order.Items.Select(item => new OrderItemDto
				{
					ProductName = item.Product.Name,
					Image = item.Product.Images,
					Quantity = item.Quantity,
					Price = item.UnitPrice
				}).ToList()
			}).ToList();

			return result;
		}
	}

}

