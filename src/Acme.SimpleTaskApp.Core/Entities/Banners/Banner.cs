using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Entities.Banners
{
    [Table("AppBanners")]
    public  class Banner : AuditedEntity<int>
    {
        public string Title { get; set; }
        public string ImageUrl { get; set; } 
        public string Link { get; set; } // đường dẫn link banner
        public int DisplayOrder { get; set; } // thứ tự hiển thị
        public bool IsActive { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
