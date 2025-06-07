using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Carts.Dto
{
    public class CartItemDto : EntityDto<int>
    {
        public long UserId { get; set; }
        public string ProductName { get; set; }

        public string Images { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
        public int ProductId { get; set; }

        public int CartItemId { get; set; }
    }
}
