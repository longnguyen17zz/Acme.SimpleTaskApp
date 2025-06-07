using Acme.SimpleTaskApp.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Acme.SimpleTaskApp.Products;
using Acme.SimpleTaskApp.Web.Models.Products;
using Abp.Application.Services.Dto;
using Acme.SimpleTaskApp.Categories;
using System;


namespace Acme.SimpleTaskApp.Web.Controllers;
public class ProductsController : SimpleTaskAppControllerBase
{
    private readonly IProductAppService _productAppService;
    private readonly ICategoryAppService _categoryAppService;


    public ProductsController(IProductAppService productAppService, ICategoryAppService categoryAppService)
    {
        _productAppService = productAppService;
        _categoryAppService = categoryAppService;
    }

    public async Task<ActionResult> Index()
    {
        var output = await _productAppService.GetAll();
        var model = new ProductListViewModel(output.Items);
        return View(model);
    }


    public async Task<ActionResult> CreateProduct()
    {
        return View();
    }

    public async Task<ActionResult> EditModal(int productId)
    {
        var product = await _productAppService.GetByIdAsync(new EntityDto<int>(productId));
        var categories = await _categoryAppService.GetAllAsync();
        var model = new EditProductViewModel
        {
            Product = product,
            Categories = categories.Items

        };
        return PartialView("_EditModal", model);

    }

}
