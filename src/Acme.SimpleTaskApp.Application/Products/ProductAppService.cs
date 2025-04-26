using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Timing;
using Abp.UI;
using Acme.SimpleTaskApp.Entities.Products;
using Acme.SimpleTaskApp.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;

using System.Threading.Tasks;
using TaskListDto = Acme.SimpleTaskApp.Products.Dto.TaskListDto;
namespace Acme.SimpleTaskApp.Products

{
    //[AbpAuthorize]
    public class ProductAppService :  SimpleTaskAppAppServiceBase, IProductAppService
    {
        private readonly IRepository<Product,Guid> _productRepository;

        public ProductAppService(IRepository<Product, Guid> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ListResultDto<ProductDto>> GetAll()
        {
            var products = await _productRepository
                .GetAll()
                .ToListAsync();

            var result = products.Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Images = p.Images,
                StockQuantity = p.StockQuantity,
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
                StockQuantity = input.StockQuantity,
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

            await _productRepository.InsertAsync(product);
        }

        public async Task<ProductDto> GetByIdAsync(EntityDto<Guid> input)
        {
            var product = await _productRepository.GetAsync(input.Id);
            return ObjectMapper.Map<ProductDto>(product);
        }
        
        [AbpAuthorize("Pages.Products.Edit")]

        [HttpPost]
        public  async Task UpdateProductData([FromForm]  UpdateProductDto input)
        {
            var product = await _productRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            if (product == null)
            {
                throw new UserFriendlyException("Sản phẩm không tồn tại.");
            }

            product.Name = input.Name;
            product.Description = input.Description;
            product.Price = input.Price;
            product.StockQuantity = input.StockQuantity;
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
        public async Task DeleteAsync(EntityDto<Guid> input)
        {
            await _productRepository.DeleteAsync(input.Id);
        }
        public async Task<PagedResultDto<ProductDto>> GetPagedAsync(ProductInput input)
        {
            try
            {
                var query = _productRepository
                    .GetAll()
                    .Include(p => p.Category)
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
                    StockQuantity = p.StockQuantity,
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

        public async Task<PagedResultDto<ProductDto>> GetPagedForUserAsync(ProductInputUser input)
        {
            var query = _productRepository
                .GetAll()
                .Include(p => p.Category)
                .Where(p => p.StockQuantity > 0);

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
                StockQuantity = p.StockQuantity,
                CreationTime = p.CreationTime,
                CategoryName = p.Category?.Name ?? "Không có danh mục"
            }).ToList();

            return new PagedResultDto<ProductDto>(totalCount, result);
        }
        
    }
}