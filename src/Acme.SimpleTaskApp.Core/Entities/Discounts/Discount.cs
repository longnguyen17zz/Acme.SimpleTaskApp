using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Entities.Categories;
using Acme.SimpleTaskApp.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Entities.Discounts
{
    [Table("AppDiscounts")]
    public class Discount : AuditedEntity<int>
    {
        public string Title { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string CategoryId { get; set; }
        public Category Category { get; set; }
        public int DiscountPercent { get; set; } // Giảm gía %
        public decimal DiscountAmount { get; set; } // Giảm giá cố định
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
    }
}
