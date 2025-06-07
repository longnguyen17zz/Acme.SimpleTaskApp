using Acme.SimpleTaskApp.Controllers;
using Acme.SimpleTaskApp.Orders;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Web.Controllers
{
    public class OrdersController : SimpleTaskAppControllerBase
    {
        private readonly IOrderAppService _orderAppService;


        public OrdersController(IOrderAppService orderAppService)
        {
            _orderAppService = orderAppService;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<FileResult> DownloadExcel()
        {
            var fileDto = await _orderAppService.ExportToExcel();
            return File(fileDto.FileBytes, fileDto.FileType, fileDto.FileName);
        }

    }
}
