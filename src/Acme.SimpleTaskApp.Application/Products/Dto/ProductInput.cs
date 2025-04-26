using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;


namespace Acme.SimpleTaskApp.Products.Dto
{
    public class ProductInput : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string Keyword { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public string CategoryId { get; set; }

        public string CategoryName { get; set; }
        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "name DESC";
            }

        }
    }
}
