using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Entities.Batches;
//using Acme.SimpleTaskApp.Entities.Batchs;
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
	public class Stock : FullAuditedEntity<int>
	{
		public int ProductId { get; set; }
		[ForeignKey("ProductId")]
		public Product Product { get; set; }
		public int BatchId { get; set; }
		[ForeignKey("BatchId")]
		public Batch Batch { get; set; }
		public int? InitQuantity { get; set; }
		public int? SellQuantity { get; set; }
		public decimal InputPrice { get; set; } // bỏ quả xử lý giá niêm yết
		public DateTime DateManufacture { get; set; }

	}
}
