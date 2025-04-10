﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Acme.SimpleTaskApp.Entities.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.SimpleTaskApp.Products.Dto
{
    [AutoMapFrom(typeof(Product))]
    public class ProductDto : EntityDto<long>
    {
        public int  Id { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public string Images { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public string Category_Id { get; set; }

        public DateTime CreationTime { get; set; }

    }
}
