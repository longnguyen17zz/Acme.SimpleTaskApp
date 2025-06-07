using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Authorization.Users;
using Acme.SimpleTaskApp.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Entities.ProductReviews
{
    [Table("AppProductReviews")]
    public class ProductReview : AuditedEntity<int>
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; }
    }
}
