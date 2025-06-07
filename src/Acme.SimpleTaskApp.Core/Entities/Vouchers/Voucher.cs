using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Entities.Vouchers
{
    [Table("AppVouchers")]
    public  class Voucher : AuditedEntity<int>
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal? DiscountAmount { get; set; } // Giá giảm cố định
        public int? DiscountPercent { get; set; } // Giá gỉam theo %
        public decimal MinOrderValue { get; set; } // Giá trị tối thiểu của đơn hàng 
        public int Quantity { get; set; } // Số lượng `     
        public DateTime ExpiryDate { get; set; } // Ngày hết hạn 
        public bool IsActive { get; set; }
    }
}
