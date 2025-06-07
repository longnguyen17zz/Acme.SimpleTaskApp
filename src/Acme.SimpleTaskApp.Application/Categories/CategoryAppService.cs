using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.UI;
using Acme.SimpleTaskApp.Categories.Dto;
using Acme.SimpleTaskApp.Entities.Categories;
using Acme.SimpleTaskApp.Entities.Products;
using Acme.SimpleTaskApp.Products.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Categories
{
    public class CategoryAppService : SimpleTaskAppAppServiceBase, ICategoryAppService
    {
        private readonly IRepository<Category, string> _categoryRepository;
        private readonly IRepository<Product> _productRepository;


        public CategoryAppService(IRepository<Category, string> categoryRepository, IRepository<Product> productRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }

        public async Task<ListResultDto<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository
                .GetAll()
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Code= c.Code,
                    Name = c.Name
                })
                .ToListAsync();

            return new ListResultDto<CategoryDto>(categories);
        }

        private async Task<string> GenerateCategoryCodeAsync()
        {
            var count = await _categoryRepository.CountAsync();
            return $"CAT{(count + 1).ToString("D3")}";
        }

        public async Task CreateAsync(CategoryDto input)
        {
            var Id = await GenerateCategoryCodeAsync();
            var category = new Category
            {
                Id = Id,
                Name = input.Name,
                Code = input.Code,
            };
            
            await _categoryRepository.InsertAsync(category);
        }
        public async Task<CategoryDto> GetByIdAsync(EntityDto<string> input)
        {
            var category = await _categoryRepository.GetAsync(input.Id);
            return new CategoryDto
            {
                Id = category.Id,
                Code = category.Code,
                Name = category.Name
            };
        }

        public async Task UpdateAsync(CategoryDto input)
        {
            var category = await _categoryRepository.FirstOrDefaultAsync(p => p.Id == input.Id);
            if (category == null)
            {
                throw new UserFriendlyException("Danh mục không tồn tại.");
            }

            var isUsed = await _productRepository.GetAll()
       .AnyAsync(p => p.CategoryId == input.Id);

            if (isUsed)
            {
                throw new UserFriendlyException("Không thể cập nhật danh mục vì đang được sử dụng bởi sản phẩm.");
            }


            //category.Id = input.Id;
            category.Name = input.Name;
            category.Code = input.Code;

            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(EntityDto<string> input)
        {
            var isUsed = await _productRepository
       .GetAll()
       .AnyAsync(p => p.CategoryId == input.Id);

            if (isUsed)
            {
                throw new UserFriendlyException("Không thể xóa danh mục vì đang được sử dụng bởi sản phẩm.");
            }
            await _categoryRepository.DeleteAsync(input.Id);
        }


        public async Task<PagedResultDto<CategoryDto>> GetPagedAsync(CategorytInput input)
        {
            try
            {
                var query = _categoryRepository.GetAll();

                if (!string.IsNullOrWhiteSpace(input.Keyword))
                {
                    query = query.Where(p => p.Name.Contains(input.Keyword));
                }

                var totalCount = await query.CountAsync();

                var categories = await query
                    .OrderBy(input.Sorting)
                    .PageBy(input)
                    .ToListAsync();

                var result = categories.Select(p => new CategoryDto
                {
                    Id = p.Id,
                   Code = p.Code,
                    Name = p.Name,
                    
                }).ToList();

                return new PagedResultDto<CategoryDto>(totalCount, result);
            }
            catch (Exception ex)
            {
                // Ghi log hoặc debug tại đây
                throw new UserFriendlyException("Lỗi xử lý GetPagedAsync: " + ex.Message, ex);
            }
        }
    }
}
