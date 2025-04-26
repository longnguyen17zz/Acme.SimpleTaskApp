using Abp.Domain.Entities;


namespace Acme.SimpleTaskApp.Categories.Dto
{
    public class CreateCategoryDto : Entity<string>
    {
        public string Name { get; set; }
    }
}
