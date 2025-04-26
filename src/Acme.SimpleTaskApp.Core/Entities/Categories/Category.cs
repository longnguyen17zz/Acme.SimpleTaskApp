using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

using Acme.SimpleTaskApp.Entities.Products;

namespace Acme.SimpleTaskApp.Entities.Categories
{
    [Table("AppCategories")]
    public class Category : Entity<string>, IHasCreationTime
    {
        public const int MaxNameLength = 256;
       

        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        public string Code { get; set; }

        public ICollection<Product> Products { get; set; }

        public DateTime CreationTime { get; set; }

        public Category()
        {
            CreationTime = Clock.Now;
        }

        public Category( string name, string code )
            : this()
        {
            Name = name;
            Code = code; 
        }
    }
}
