using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Acme.SimpleTaskApp.Stocks.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Stocks
{
    public interface IStockAppService : IApplicationService
    {
        public Task<PagedResultDto<StockDto>> GetPagedAsync(StockInput input);

        public Task<StockDto> GetByIdAsync(EntityDto<int> input);

        public Task UpdateStock(UpdateStockDto input);

        public Task<StockDto> GetStockQuantity(int input);
    }

}
