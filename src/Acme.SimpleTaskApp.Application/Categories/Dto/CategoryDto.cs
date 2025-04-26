

using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Acme.SimpleTaskApp.Entities.Categories;

namespace Acme.SimpleTaskApp.Categories.Dto
{
    [AutoMapFrom(typeof(Category))]
    public  class CategoryDto: EntityDto<string>
    {
        public string Name { get; set; }

        public string Code { get; set; }

    }
}
