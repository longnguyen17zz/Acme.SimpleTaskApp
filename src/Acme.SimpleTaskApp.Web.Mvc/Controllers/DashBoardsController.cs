using Abp.AspNetCore.Mvc.Authorization;
using Acme.SimpleTaskApp.Controllers;
using Acme.SimpleTaskApp.Orders;
using Acme.SimpleTaskApp.Web.Models.Dashboards;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Web.Controllers
{
	[AbpMvcAuthorize]

	public class DashBoardsController : SimpleTaskAppControllerBase
    {
        private readonly IOrderAppService _orderAppService;

        public DashBoardsController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;
        }
        public async Task<ActionResult> Index()
        {
            var dtoList = await _orderAppService.GetTopSellingProductsAsync();

            var viewModelList = dtoList.Select(x => new TopSellingProductViewModel
            {
                Name = x.Name,
                Price = x.Price,
                CategoryName = x.CategoryName,
                Image = x.Image,
                TotalSold = x.TotalSold
            }).ToList();

            return View(viewModelList); 
        }
    }
}
