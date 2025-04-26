using Abp.Application.Services.Dto;
using Acme.SimpleTaskApp.Categories;
using Acme.SimpleTaskApp.Categories.Dto;
using Acme.SimpleTaskApp.Controllers;
using Acme.SimpleTaskApp.Web.Models.Categories;
using Acme.SimpleTaskApp.Web.Models.Products;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Web.Controllers;

    public class CategoriesController : SimpleTaskAppControllerBase
    {
        private readonly ICategoryAppService _categoryAppService;

        public CategoriesController(ICategoryAppService categoryAppService)
        {
            _categoryAppService = categoryAppService;
        }
        public async Task<ActionResult> Index(CategoryDto input)
        {
            var output = await _categoryAppService.GetAllAsync();
            var model = new CategoryListViewModel(output.Items);
            return View(model);
        }

    public async Task<ActionResult> EditModal(string categoryId)
    {
        var category = await _categoryAppService.GetByIdAsync(new EntityDto<string>(categoryId));
        var model = new EditCategoryViewModel
        {
            Category = category,
        };
        return PartialView("_EditModal", model);
    }

}

