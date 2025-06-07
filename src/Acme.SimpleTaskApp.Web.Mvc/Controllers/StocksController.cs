using Abp.Application.Services.Dto;
using Acme.SimpleTaskApp.Controllers;
using Acme.SimpleTaskApp.Stocks;
using Acme.SimpleTaskApp.Web.Models.Products;
using Acme.SimpleTaskApp.Web.Models.Stocks;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Web.Controllers
{
    public class StocksController : SimpleTaskAppControllerBase
    {

        private readonly IStockAppService _stockAppService;


        public StocksController(IStockAppService stockAppService)
        {
            _stockAppService = stockAppService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> EditModal(int stockId)
        {
            var stock = await _stockAppService.GetByIdAsync(new EntityDto<int>(stockId));
            //var categories = await _categoryAppService.GetAllAsync();
            var model = new EditStockViewModel
            {
                Stock = stock,
                //Categories = categories.Items

            };
            return PartialView("_EditModal", model);

        }
    }
}
