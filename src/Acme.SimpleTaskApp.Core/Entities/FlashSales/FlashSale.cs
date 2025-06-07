using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Entities.FlashSaleItems;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Entities.FlashSales
{
    [Table("AppFlashSales")]
    public class FlashSale : FullAuditedEntity<int>
    {
        public string Title { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsActive { get; set; }

        public ICollection<FlashSaleItem> Items { get; set; }
    }
}
