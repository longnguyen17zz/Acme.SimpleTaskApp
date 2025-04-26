using Acme.SimpleTaskApp.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Acme.SimpleTaskApp.Web.Controllers
{
    public class OrdersController : SimpleTaskAppControllerBase
    {
        public IActionResult OrderSuccess()
        {
            return View();
        }
    }
}
