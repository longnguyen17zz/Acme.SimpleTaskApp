using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Products.Dto
{
    public class ProductInputUser : PagedAndSortedResultRequestDto, IShouldNormalize
    {
        public string Keyword { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string UserId { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime DESC";
            }

        }

        public ProductInputUser()
        {
            MaxResultCount = 15;
        }
    }
}
