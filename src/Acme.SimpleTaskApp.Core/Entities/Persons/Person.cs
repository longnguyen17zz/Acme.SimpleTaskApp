using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Acme.SimpleTaskApp.Entities.Persons
{
    [Table("AppPersons")]
    public class Person : AuditedEntity<Guid>
    {
        public const int MaxNameLength = 32;

        [Required]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        public Person()
        {

        }

        public Person(string name)
        {
            Name = name;
        }
    }
}
