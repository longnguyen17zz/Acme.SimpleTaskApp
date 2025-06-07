using Acme.SimpleTaskApp.FlashSales.Dto;
using Acme.SimpleTaskApp.Products.Dto;
using Acme.SimpleTaskApp.Stocks.Dto;
using System.Collections.Generic;

namespace Acme.SimpleTaskApp.Web.Models.HomepageProducts
{
    public class ProductDetailViewModel
    {
        public ProductDto Product { get; set; }

        public StockDto Stock   { get; set; }

        public FlashSaleDto FlashSale { get; set; }

        public ProductDetailViewModel(ProductDto product,StockDto stock, FlashSaleDto flashSale)
        {
            Product = product;
            Stock = stock;
            FlashSale = flashSale;
        }
    }
}
