using System;

namespace Acme.SimpleTaskApp.Web.Models.OrderSuccess
{
    public class OrderSuccessViewModel
    {
        public int OrderCode { get; set; }             // Mã đơn hàng
        public DateTime OrderDate { get; set; }           // Ngày đặt
        public string PaymentMethod { get; set; }         // Phương thức
        public decimal TotalAmount { get; set; }          // Tổng tiền
        public string Status { get; set; }                // Trạng thái
        public DateTime EstimatedDeliveryDate { get; set; } // Dự kiến giao
    }
}
