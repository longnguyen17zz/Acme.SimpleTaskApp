using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Acme.SimpleTaskApp.FlashSales.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.FlashSales
{
	public interface IFlashSaleAppService : IApplicationService
	{

		public Task<List<FlashSaleDto>> GetAllAsync();

		public Task<FlashSaleDto> GetAsync(int id);

		//public Task CreateOrUpdateAsync(FlashSaleDto input);

		public Task DeleteAsync(int id);

		public Task<PagedResultDto<FlashSaleDto>> GetAllPaged(GetAllFlashSaleInput input);

		public Task ApplyForAsync(ApplyFlashSaleDto input);

		Task<List<FlashSaleItemDto>> GetFlashSaleItemsByFlashSaleId(int flashSaleId);

		Task<List<FlashSaleDto>> GetAllIsActiveAsync();
		Task CreateAsync(FlashSaleDto input);

		Task UpdateAsync(FlashSaleDto input);


	}
}
