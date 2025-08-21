using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Stocks.Dto
{
    public class StockInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string Keyword { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public int BatchId { get; set; }

		    public void Normalize()
            {
                if (string.IsNullOrEmpty(Sorting))
                {
                    Sorting = "CreationTime DESC";
                }

            }
        }
}
