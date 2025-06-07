using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Acme.SimpleTaskApp.Entities.Carts;
using Acme.SimpleTaskApp.Entities.Products;
using Acme.SimpleTaskApp.Entities.Stocks;
using Acme.SimpleTaskApp.Orders.Dto;
using Acme.SimpleTaskApp.Products.Dto;
using Acme.SimpleTaskApp.Stocks.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Stocks
{
    public class StockAppService : SimpleTaskAppAppServiceBase, IStockAppService
    {

        private readonly IRepository<Stock> _stockRepository;
        private readonly IRepository<Product> _productRepository;


        public StockAppService(IRepository<Stock> stockRepository, IRepository<Product> productRepository)
        {
            _stockRepository = stockRepository;
            _productRepository = productRepository;
        }

        public async Task<PagedResultDto<StockDto>> GetPagedAsync(StockInput input)
        {

            var query = _stockRepository
                .GetAll()
                .Include(p => p.Product)
                .ThenInclude(i => i.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(input.Keyword))
            {
                query = query.Where(p => p.Product.Name.ToLower().Contains(input.Keyword.ToLower()));
            }

            if (input.FromDate.HasValue)
            {
                query = query.Where(x => x.CreationTime >= input.FromDate.Value);
            }

            if (input.ToDate.HasValue)
            {
                query = query.Where(x => x.CreationTime <= input.ToDate.Value.AddDays(1));
            }

            var totalCount = await query.CountAsync();
            var stocks = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var result = stocks.Select(p => new StockDto
            {
               Id = p.Id,
               StockQuantity = p.StockQuantity ?? 0,
               ProductName = p.Product.Name,
               CategoryName = p.Product.Category.Name,
               LastModificationTime = p.LastModificationTime
            }).ToList();
            return new PagedResultDto<StockDto>(totalCount, result);
        }

        public async Task<StockDto> GetByIdAsync(EntityDto<int> input)
        {
            var stock = await _stockRepository.GetAll()
           .Include(s => s.Product)
           .ThenInclude(p => p.Category)
           .FirstOrDefaultAsync(s => s.Id == input.Id);
            var result = new StockDto
            {
                Id = input.Id,
                StockQuantity = stock.StockQuantity ?? 0,
                ProductName = stock.Product.Name ?? "Không có tên",
                CategoryName = stock.Product.Category.Name ?? "Không có danh mục",
                LastModificationTime = stock.LastModificationTime
            };
            return result;
        }

        [HttpPost]
        public async Task UpdateStock(UpdateStockDto input)
        {
            var stock = await _stockRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            stock.StockQuantity = input.StockQuantity;
            stock.LastModificationTime = DateTime.Now;
            await _stockRepository.UpdateAsync(stock);
        }

        public async Task<StockDto> GetStockQuantity(int input)
        {
            var product = await _productRepository.GetAll().Include(p =>p.Stock).FirstOrDefaultAsync(p=> p.Id == input);
            var result = new StockDto
            {
                StockQuantity = product.Stock.StockQuantity ?? 0,
            }
            ;
            return result;
        }
        public async Task Create()
        {
        }
    }
}
