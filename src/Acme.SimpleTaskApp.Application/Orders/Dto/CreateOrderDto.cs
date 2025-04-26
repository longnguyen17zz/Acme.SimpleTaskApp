using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Orders.Dto
{
   public class CreateOrderDto
{
    public string PaymentMethod { get; set; } // Ví dụ: "Tiền mặt", "Chuyển khoản"
    public List<CreateOrderItemInput> Items { get; set; }
}

public class CreateOrderItemInput
{
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

}
