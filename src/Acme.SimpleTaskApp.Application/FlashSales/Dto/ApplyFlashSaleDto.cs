using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.FlashSales.Dto
{
    public  class ApplyFlashSaleDto : Entity<int>
    {
        public int FlashSaleId { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public decimal FlashPrice { get; set; }
        public int Quantity { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
