using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Entities.Stocks
{
    [Table("AppStocks")]
    public class Stock : AuditedEntity<int>
    {
        public int ProductId { get; set; }
        public int? StockQuantity { get; set; }
        public DateTime LastUpdated { get; set; }

        public Product Product { get; set; }
    }
}
