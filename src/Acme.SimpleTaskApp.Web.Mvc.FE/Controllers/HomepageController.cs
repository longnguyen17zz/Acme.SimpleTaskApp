using Abp.Application.Services.Dto;
using Acme.SimpleTaskApp.Authorization.Users;
using Acme.SimpleTaskApp.Categories;
using Acme.SimpleTaskApp.Controllers;
using Acme.SimpleTaskApp.Entities.FlashSales;
using Acme.SimpleTaskApp.FlashSales;
using Acme.SimpleTaskApp.FlashSales.Dto;
using Acme.SimpleTaskApp.Orders;
using Acme.SimpleTaskApp.Products;
using Acme.SimpleTaskApp.Products.Dto;
using Acme.SimpleTaskApp.Stocks;
using Acme.SimpleTaskApp.Stocks.Dto;
using Acme.SimpleTaskApp.Web.Models.HomepageProducts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Web.Controllers
{
    public class HomepageController : SimpleTaskAppControllerBase
    {

        public readonly IProductAppService _productAppService;
        public readonly ICategoryAppService _categoryAppService;
        public readonly IStockAppService _stockAppService;
        public readonly IOrderAppService _orderAppService;
        public readonly IFlashSaleAppService _flashSaleAppService;





        public HomepageController(IProductAppService productAppService, ICategoryAppService categoryAppService, UserManager userManager, IStockAppService stockAppService, IOrderAppService orderAppService, IFlashSaleAppService flashSaleAppService)
        {
            _productAppService = productAppService;
            _categoryAppService = categoryAppService;
            _stockAppService = stockAppService;
            _orderAppService = orderAppService;
            _flashSaleAppService = flashSaleAppService;
        }

        public async Task<IActionResult> Index(ProductInputUser input)
        {
            var products = await _productAppService.GetPagedForUserAsync(input);
            var categories = await _categoryAppService.GetAllAsync();
            var productTops = await _orderAppService.GetTopSellingProductsAsync();

            int currentPage = (input.SkipCount / input.MaxResultCount) + 1;
            int totalPages = (int)Math.Ceiling((double)products.TotalCount / input.MaxResultCount);
            var model = new HomepageViewModel(categories.Items,products.Items, productTops,currentPage, input.MaxResultCount,totalPages,input.Keyword,input.CategoryId,input.CategoryId);
            ViewBag.Keyword = input.Keyword;

            //@ViewBag.Keyword
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("_ProductListPartial", model);
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetail(int id)
        {
            var product = await _productAppService.GetByIdAsync(new EntityDto<int>(id));
            var stock = await _stockAppService.GetStockQuantity(id);
            var flashSales = await _flashSaleAppService.GetAllIsActiveAsync();
            //var model = new ProductDetailViewModel(product,stock, flashSale);
            //return View(model);
            var matchedFlashSale = flashSales
                .FirstOrDefault(fs => fs.Items.Any(i => i.ProductId == id));

            var viewModel = new ProductDetailViewModel
            (
                product,
                stock,
                matchedFlashSale != null
                    ? new FlashSaleDto
                    {
                        Id = matchedFlashSale.Id,
                        Name = matchedFlashSale.Name,
                        StartTime = matchedFlashSale.StartTime,
                        EndTime = matchedFlashSale.EndTime,
                        IsActive = matchedFlashSale.IsActive,
                        CreationTime = matchedFlashSale.CreationTime,
                        Items = matchedFlashSale.Items
                            .Where(i => i.ProductId == id) // chỉ giữ item của sản phẩm này
                            .Select(i => new FlashSaleItemDto
                            {
                                Id = i.Id,
                                ProductId = i.ProductId,
                                ProductName = i.ProductName,
                                FlashSaleId = i.FlashSaleId,
                                Sold = i.Sold,
                                OriginPrice = i.OriginPrice,
                                SalePrice = i.SalePrice,
                                QuantityLimit = i.QuantityLimit
                            }).ToList()
                    }
                    : null
            );

            return View(viewModel);
        }
    }
}
