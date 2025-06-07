using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Authorization.Users;
using Acme.SimpleTaskApp.Entities.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Entities.HistoryViews
{
    [Table("AppHistoryViews")]
    public class HistoryView : AuditedEntity<int>
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime ViewedAt { get; set; }
    }
}
