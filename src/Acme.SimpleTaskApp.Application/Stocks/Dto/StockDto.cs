using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Stocks.Dto
{
    public class StockDto : Entity<int>
    {
        public int StockQuantity { get; set; }
        public string CategoryName { get; set; }
       public string ProductName { get; set; }

        public DateTime? LastModificationTime { get; set; }
    }
}
