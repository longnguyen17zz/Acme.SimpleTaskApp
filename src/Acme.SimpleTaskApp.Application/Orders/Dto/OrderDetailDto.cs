using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Orders.Dto
{
    public class OrderDetailDto
    {
        public int OrderCode { get; set; }             // Mã đơn hàng
        public DateTime OrderDate { get; set; }           // Ngày đặt
        public string PaymentMethod { get; set; }         // Phương thức
        public decimal TotalAmount { get; set; }          // Tổng tiền
        public string Status { get; set; }                // Trạng thái
        public DateTime EstimatedDeliveryDate { get; set; } // Dự kiến giao
    }
}
