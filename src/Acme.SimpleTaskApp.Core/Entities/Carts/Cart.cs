using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Acme.SimpleTaskApp.Authorization.Users;
using Acme.SimpleTaskApp.Entities.CartItems;
using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;


namespace Acme.SimpleTaskApp.Entities.Carts
{
    [Table("AppCarts")]

    public class Cart : FullAuditedEntity<int>
    {

        public long UserId { get; set; } // khóa ngoại    
        [ForeignKey("UserId")]
        public virtual User User { get; set; }
        public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
        public DateTime CreationTime { get; set; }
        public Cart(long userId)
          
        {
            UserId = userId;
           
        }
    }
}
