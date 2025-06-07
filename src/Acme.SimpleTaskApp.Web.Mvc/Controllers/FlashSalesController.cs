using Acme.SimpleTaskApp.Controllers;
using Acme.SimpleTaskApp.FlashSales;
using Acme.SimpleTaskApp.FlashSales.Dto;
using Acme.SimpleTaskApp.Products;
using Acme.SimpleTaskApp.Web.Models.FlashSales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Web.Controllers
{
	public class FlashSalesController : SimpleTaskAppControllerBase
	{
		private readonly IProductAppService _productAppSevice;
		private readonly IFlashSaleAppService _flashSaleAppSevice;


		public FlashSalesController(IProductAppService productAppSevice, IFlashSaleAppService flashSaleAppSevice)
		{
			_productAppSevice = productAppSevice;
			_flashSaleAppSevice = flashSaleAppSevice;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> ApplyFor(int FlashSaleId, int ProductId, decimal FlashPrice, int Quantity)
		{
			var input = new ApplyFlashSaleDto
			{

				FlashSaleId = FlashSaleId,
				ProductId = ProductId,
				FlashPrice = FlashPrice,
				Quantity = Quantity,
				// Nếu có Id (để update), gán thêm, hoặc bỏ qua
			};
			await _flashSaleAppSevice.ApplyForAsync(input);
			return RedirectToAction("Index");
		}

		public async Task<ActionResult> EditModal(int flashSaleId)
		{
			var flashSale = await _flashSaleAppSevice.GetAsync(flashSaleId);
			
			var model = new FlashSaleEditListViewModel
			{
				Id = flashSale.Id,
				Title = flashSale.Name,
				StartTime = flashSale.StartTime,
				EndTime = flashSale.EndTime,
				IsActive = flashSale.IsActive,
				//IsActiveSelect = items

			};
			return PartialView("_EditModal", model);
		}
		public IActionResult DetailFlashSale()
		{
			return View();
		}


	}
}
