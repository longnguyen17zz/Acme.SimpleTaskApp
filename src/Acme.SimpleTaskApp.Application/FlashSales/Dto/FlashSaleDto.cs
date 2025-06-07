using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.FlashSales.Dto
{
	public class FlashSaleDto
	{

		public int Id { get; set; }
		public string Name { get; set; }

		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public bool IsActive { get; set; }


		public DateTime CreationTime { get; set; }

		public List<FlashSaleItemDto> Items { get; set; } = new List<FlashSaleItemDto>();

	}
	public class FlashSaleItemDto
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public string ProductName { get; set; }
		public int FlashSaleId { get; set; }

		public int Sold { get; set; }
		public decimal OriginPrice { get; set; }
		public decimal SalePrice { get; set; }
		public int QuantityLimit { get; set; }
	}
}
