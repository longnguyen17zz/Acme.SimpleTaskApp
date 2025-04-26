using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;


namespace Acme.SimpleTaskApp.Categories.Dto
{
    public class CategorytInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {

        public string Keyword { get; set; }

        public void  Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "name DESC";
            }
        }
    }
}
