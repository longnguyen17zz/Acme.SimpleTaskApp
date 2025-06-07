using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Stocks.Dto
{
    public class UpdateStockDto : EntityDto<int>
    {
        public int StockQuantity { get; set; }
    }
}
