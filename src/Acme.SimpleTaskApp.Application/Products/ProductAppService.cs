using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Timing;
using Abp.UI;
using Acme.SimpleTaskApp.Entities.Products;
using Acme.SimpleTaskApp.Entities.Stocks;
using Acme.SimpleTaskApp.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;

using System.Threading.Tasks;
using TaskListDto = Acme.SimpleTaskApp.Products.Dto.TaskListDto;
namespace Acme.SimpleTaskApp.Products
{
	//[AbpAuthorize]
	public class ProductAppService : SimpleTaskAppAppServiceBase, IProductAppService
	{
		private readonly IRepository<Product> _productRepository;
		private readonly IRepository<Stock> _stockRepository;
		public ProductAppService(IRepository<Product> productRepository, IRepository<Stock> stockRepository)
		{
			_productRepository = productRepository;
			_stockRepository = stockRepository;
		}

		// Hàm sử dụng bên admin
		public async Task<ListResultDto<ProductDto>> GetAll()
		{
			var products = await _productRepository
					.GetAll()
					.Include(p => p.Stock)
					.ToListAsync();

			var result = products.Select(p => new ProductDto
			{
				Id = p.Id,
				Name = p.Name,
				Description = p.Description,
				Price = p.Price,
				Images = p.Images,
				StockQuantity = p.Stock.StockQuantity ?? 0,
				CreationTime = p.CreationTime,
			}).ToList();

			return new ListResultDto<ProductDto>(result);
		}
		private string GetSharedImagePath()
		{
			return Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), @"..\Product\ProductImages"));
		}
		[AbpAuthorize("Pages.Products.Create")]
		public async System.Threading.Tasks.Task Create([FromForm] CreateProductDto input)
		{

			var product = new Product
			{
				Name = input.Name,
				Description = input.Description,
				Price = input.Price,
				CategoryId = input.CategoryId,
				CreationTime = Clock.Now
			};

			if (input.Images != null && input.Images.Length > 0)
			{
				var fileName = Guid.NewGuid().ToString() + Path.GetExtension(input.Images.FileName);
				var sharedPath = Path.Combine(GetSharedImagePath(), fileName);

				using (var stream = new FileStream(sharedPath, FileMode.Create))
				{
					await input.Images.CopyToAsync(stream);
				}

				// Đường dẫn dùng để hiển thị ảnh trong trình duyệt
				product.Images = "/ProductImages/" + fileName;
			}
			// Thêm sản phẩm trước để có ProductId
			var createdProduct = await _productRepository.InsertAsync(product);
			await CurrentUnitOfWork.SaveChangesAsync(); // Đảm bảo ProductId được tạo ra

			// Tạo stock tương ứng
			var stock = new Stock
			{
				ProductId = createdProduct.Id,
				StockQuantity = 0, // hoặc bạn có thể nhận từ input nếu có
				CreationTime = Clock.Now
			};

			await _stockRepository.InsertAsync(stock);
		}
		public async Task<ProductDto> GetByIdAsync(EntityDto<int> input)
		{
			var product = await _productRepository.GetAsync(input.Id);
			return ObjectMapper.Map<ProductDto>(product);
		}

		[AbpAuthorize("Pages.Products.Edit")]

		[HttpPost]
		public async Task UpdateProductData([FromForm] UpdateProductDto input)
		{
			var product = await _productRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
			if (product == null)
			{
				throw new UserFriendlyException("Sản phẩm không tồn tại.");
			}

			product.Name = input.Name;
			product.Description = input.Description;
			product.Price = input.Price;
			product.CategoryId = input.CategoryId;

			if (input.Images != null && input.Images.Length > 0)
			{
				// 1. Xoá ảnh cũ nếu tồn tại
				if (!string.IsNullOrEmpty(product.Images))
				{
					var oldPath = Path.Combine(GetSharedImagePath(), Path.GetFileName(product.Images));
					if (System.IO.File.Exists(oldPath))
					{
						System.IO.File.Delete(oldPath);
					}
				}

				// 2. Lưu ảnh mới
				var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(input.Images.FileName);
				var newPath = Path.Combine(GetSharedImagePath(), newFileName);

				using (var stream = new FileStream(newPath, FileMode.Create))
				{
					await input.Images.CopyToAsync(stream);
				}

				// 3. Cập nhật đường dẫn hiển thị
				product.Images = "/ProductImages/" + newFileName;
			}

			await _productRepository.UpdateAsync(product);
		}
		[AbpAuthorize("Pages.Products.Delete")]
		public async Task DeleteAsync(EntityDto<int> input)
		{
			await _productRepository.DeleteAsync(input.Id);
		}
		public async Task<PagedResultDto<ProductDto>> GetPagedAsync(ProductInput input)
		{
			try
			{
				var query = _productRepository
						.GetAll()
						.Include(c => c.Category)
						.AsQueryable();

				if (!string.IsNullOrWhiteSpace(input.CategoryId))
				{
					query = query.Where(p => p.CategoryId == input.CategoryId);
				}

				if (!string.IsNullOrWhiteSpace(input.Keyword))
				{
					query = query.Where(p => p.Name.ToLower().Contains(input.Keyword.ToLower()));
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
				var products = await query
						.OrderBy(input.Sorting)
						.PageBy(input)
						.ToListAsync();

				var result = products.Select(p => new ProductDto
				{
					Id = p.Id,
					Name = p.Name,
					Description = p.Description,
					Price = p.Price,
					Images = p.Images,
					CreationTime = p.CreationTime,
					CategoryName = p.Category != null ? p.Category.Name : "Không có danh mục"
				}).ToList();
				return new PagedResultDto<ProductDto>(totalCount, result);
			}
			catch (Exception ex)
			{
				// Ghi log hoặc debug tại đây
				throw new UserFriendlyException("Lỗi xử lý GetPagedAsync: " + ex.Message, ex);
			}
		}

		// Hàm sử dụng bên user
		public async Task<PagedResultDto<ProductDto>> GetPagedForUserAsync(ProductInputUser input)
		{
			var query = _productRepository
					.GetAll()
					.Include(p => p.Stock)
					.Include(p => p.Category)
					.AsQueryable()
					.Where(p => p.Stock.StockQuantity > 0);

			if (!string.IsNullOrWhiteSpace(input.CategoryId))
			{
				query = query.Where(p => p.CategoryId == input.CategoryId);
			}

			if (!string.IsNullOrWhiteSpace(input.Keyword))
			{
				query = query.Where(p => p.Name.ToLower().Contains(input.Keyword.ToLower()));
			}

			var totalCount = await query.CountAsync();
			var products = await query
					.OrderBy(input.Sorting)
					.PageBy(input)
					.ToListAsync();

			var result = products.Select(p => new ProductDto
			{
				Id = p.Id,
				Name = p.Name,
				Description = p.Description,
				Price = p.Price,
				Images = p.Images,
				StockQuantity = p.Stock?.StockQuantity ?? 0,
				CreationTime = p.CreationTime,
				CategoryName = p.Category?.Name ?? "Không có danh mục"
			}).ToList();

			return new PagedResultDto<ProductDto>(totalCount, result);
		}

		// Hàm sử dụng cả admin cả user 
		public async Task<ProductDto> GetByIdProduct(int productId)
		{
			var product = await _productRepository.GetAsync(productId);
			return ObjectMapper.Map<ProductDto>(product);
		}
	}
}