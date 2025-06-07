using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Entities.FlashSales;
using Acme.SimpleTaskApp.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Entities.FlashSaleItems
{
    [Table("AppFlashSaleItems")]
    public class FlashSaleItem : FullAuditedEntity<int>
    {
        public int FlashSaleId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal FlashPrice { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }

        public FlashSale FlashSale { get; set; }
    }
}
