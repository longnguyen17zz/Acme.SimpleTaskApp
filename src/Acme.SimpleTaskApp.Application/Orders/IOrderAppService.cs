using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Acme.SimpleTaskApp.Common.Dto;
using Acme.SimpleTaskApp.Orders.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Orders
{
	public interface IOrderAppService : IApplicationService
	{
		Task<int> CreateOrderFromCartAsync(long userId, string shippingName, string shippingAddress, string ward, string province, string shippingPhone);

		Task<int> CreateOrderDirectAsync(long userId, int productId, int quantity, string shippingName, string shippingAddress, string ward, string province, string shippingPhone);
		Task<PagedResultDto<ShippingInfoDto>> GetPagedAsync(OrderInput input);

		Task<GetOrderDetailsOutput> GetOrderDetails(int orderId);

		Task<List<TopSellingProductDto>> GetTopSellingProductsAsync();

		Task<OrderDetailDto> GetAsync(EntityDto<int> input);

		Task<List<GetOrderDetailsOutput>> GetOrderListUser();

		Task<OrderDto> GetOrderDetail(int orderId);

		Task ConfirmDelivery(ConfirmDeliveryInput input);

		Task<FileDto> ExportToExcel();
	}
}
