using Abp.Application.Services.Dto;
using Acme.SimpleTaskApp.Controllers;
using Acme.SimpleTaskApp.Entities.Products;
using Acme.SimpleTaskApp.Products;
using Acme.SimpleTaskApp.Products.Dto;
using Acme.SimpleTaskApp.Web.Areas.UserPortal.Models.HomepageProducts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Web.Areas.UserPortal.Controllers
{
    [Area("UserPortal")]
    public class HomepageController : SimpleTaskAppControllerBase
    {

        public readonly IProductAppService _productAppService;

        public HomepageController(IProductAppService productAppService)
        {
            _productAppService = productAppService;
        }
        public async Task<ActionResult> Index()
        {
            var products = await _productAppService.GetAll();
            var model = new ProductListViewModel(products.Items);
            return View(model);
        }

        public async Task<ActionResult> Detail(Guid id)
        {
            var product = await _productAppService.GetByIdAsync(new EntityDto<Guid>(id));
            return View(product);
        }
    }
}
