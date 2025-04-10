using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Timing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Acme.SimpleTaskApp.Entities.Products;

namespace Acme.SimpleTaskApp.Entities.Categories
{
    [Table("AppCategories")]
    public class Category : Entity, IHasCreationTime
    {
        public const int MaxNameLength = 256;
        public string Id { get; set; }

        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        public DateTime CreationTime { get; set; }

        public Category()
        {
            CreationTime = Clock.Now;
        }

        public Category(string id, string name)
            : this()
        {
            Id = id;
            Name = name;
        }
    }
}
