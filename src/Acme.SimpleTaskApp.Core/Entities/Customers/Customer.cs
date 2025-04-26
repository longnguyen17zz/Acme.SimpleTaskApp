//using Abp.Domain.Entities.Auditing;
//using System.Collections.Generic;
//using System;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using Acme.SimpleTaskApp.Entities.Orders;
//using Acme.SimpleTaskApp.Entities.Carts;


//namespace Acme.SimpleTaskApp.Entities.Customers
//{
//    [Table("AppCustomers")]
//    public class Customer : FullAuditedEntity<Guid>
//    {
//        public const int MaxPasswordLength = 512;

//        [Required]
//        [MaxLength(100)]
//        public string UserName { get; set; }

//        [Required]
//        [MaxLength(100)]
//        public string EmailAddress { get; set; }

//        public string Phone { get; set; }

//        [Required]
//        [MaxLength(MaxPasswordLength)]
//        public string PasswordHash  { get; set; }

//        public string FullName { get; set; }

//        public bool IsActive { get; set; }

//        public ICollection<Order> Orders { get; set; } =  new List<Order>();

//        public ICollection<Cart> Carts  {  get; set; } = new List<Cart>();

       
//        public Customer(string userName, string emailAddress, string phone, string passwordHash, string fullName, bool isActive)
           
//        {
//            UserName = userName;
//            EmailAddress = emailAddress;
//            Phone = phone;
//            PasswordHash = passwordHash;
//            FullName = fullName;
//            IsActive = isActive;
//        }
//    }
//}
