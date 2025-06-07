using Acme.SimpleTaskApp.FlashSales.Dto;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.FlashSales
{
	public class FlashSaleEditListViewModel
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }

		public bool IsActive { get; set; }

		//public IEnumerable<SelectListItem> IsActiveSelect { get; set; }
		//public DateTime CreationTime { get; set; }
	}
}
