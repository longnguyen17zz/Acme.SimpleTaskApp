using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Timing;
using Abp.UI;
using Acme.SimpleTaskApp.Entities.Products;
using Acme.SimpleTaskApp.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TaskListDto = Acme.SimpleTaskApp.Products.Dto.TaskListDto;

namespace Acme.SimpleTaskApp.Products
{
    public class ProductAppService :  SimpleTaskAppAppServiceBase, IProductAppService
    {
        private readonly IRepository<Product> _productRepository;

        public ProductAppService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ListResultDto<TaskListDto>> GetAll(GetAllProductsDto input)
        {
            var products = await _productRepository
                .GetAll()
                .ToListAsync();

            return new ListResultDto<TaskListDto>(
                ObjectMapper.Map<List<TaskListDto>>(products)
            );
        }
        public async System.Threading.Tasks.Task Create([FromForm] CreateProductDto input)
        {
            var product = ObjectMapper.Map<Product>(input);
            product.CreationTime = Clock.Now; // Đảm bảo CreationTime được gán giá trị hợp lệ
            if (input.Images != null && input.Images.Length > 0)
            {
                // Lấy tên file và tạo đường dẫn lưu vào thư mục wwwroot/images
                var fileName = Path.GetFileName(input.Images.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/ImageProducts", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await input.Images.CopyToAsync(stream); 
                };
                product.Images = $"/img/ImageProducts/{fileName}";
            }
            await _productRepository.InsertAsync(product);
        }

        public async Task<ProductDto> GetAsync(EntityDto<int> input)
        {
            var product = await _productRepository.GetAsync(input.Id);
            return ObjectMapper.Map<ProductDto>(product);
        }

        [HttpPost]
        public  async Task UpdateProductData([FromForm]  UpdateProductDto input)
        {
            //CheckUpdatePermission();

            var product = await _productRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            if (product == null)
            {
                throw new UserFriendlyException("Sản phẩm không tồn tại.");
            }
            product.Name = input.Name;
            product.Description = input.Description;
            product.Price = input.Price;
            product.StockQuantity = input.StockQuantity;
            if (input.Images != null && input.Images.Length > 0)
            {
                // 1. Xoá ảnh cũ nếu tồn tại
                if (!string.IsNullOrEmpty(product.Images))
                {
                    var oldImagePath = Path.Combine("wwwroot", product.Images.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                // 2. Lưu ảnh mới
                var newFileName = System.Guid.NewGuid().ToString() + Path.GetExtension(input.Images.FileName);
                var newPath = Path.Combine("wwwroot/img/ImageProducts", newFileName);

                using (var stream = new FileStream(newPath, FileMode.Create))
                {
                    await input.Images.CopyToAsync(stream);
                }
                // 3. Cập nhật đường dẫn ảnh
                product.Images = "/img/ImageProducts/" + newFileName;
            }

            await _productRepository.UpdateAsync(product);
        }

       
        public async Task DeleteAsync(EntityDto<int> input)
        {
            await _productRepository.DeleteAsync(input.Id);
        }



        //public async Task<PagedResultDto<ProductDto>> GetAll(GetAllProductInput input)
        //{
        //    var query = _productRepository.GetAll()
        //        .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), p => p.Name.Contains(input.Keyword));

        //    var totalCount = await query.CountAsync();

        //    var items = await query
        //        .OrderBy(input.Sorting ?? "Name asc") // sắp xếp
        //        .PageBy(input)                        // skip + take
        //        .ToListAsync();

        //    return new PagedResultDto<ProductDto>(
        //        totalCount,
        //        ObjectMapper.Map<List<ProductDto>>(items)
        //    );
        //}

    }
}