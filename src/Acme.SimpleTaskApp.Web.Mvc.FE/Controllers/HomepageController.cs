using Abp.Application.Services.Dto;
using Acme.SimpleTaskApp.Authorization.Users;
using Acme.SimpleTaskApp.Categories;
using Acme.SimpleTaskApp.Controllers;
using Acme.SimpleTaskApp.Products;
using Acme.SimpleTaskApp.Products.Dto;
using Acme.SimpleTaskApp.Web.Models.HomepageProducts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Web.Controllers
{
    public class HomepageController : SimpleTaskAppControllerBase
    {

        public readonly IProductAppService _productAppService;
        public readonly ICategoryAppService _categoryAppService;


        public HomepageController(IProductAppService productAppService, ICategoryAppService categoryAppService, UserManager userManager)
        {
            _productAppService = productAppService;
            _categoryAppService = categoryAppService;
        }

        public async Task<IActionResult> Index(ProductInputUser input)
        {
            var products = await _productAppService.GetPagedForUserAsync(input);
            var categories = await _categoryAppService.GetAllAsync();
            int currentPage = (input.SkipCount / input.MaxResultCount) + 1;
            int totalPages = (int)Math.Ceiling((double)products.TotalCount / input.MaxResultCount);
            var model = new HomepageViewModel(categories.Items,products.Items,currentPage,input.MaxResultCount,totalPages,input.Keyword,input.CategoryId,input.CategoryId);
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductListPartial", model);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var product = await _productAppService.GetByIdAsync(new EntityDto<Guid>(id));
            var model = new ProductDetailViewModel(product);
            return View(model);
        }
    }
}
