using Abp.Zero.EntityFrameworkCore;
using Acme.SimpleTaskApp.Authorization.Roles;
using Acme.SimpleTaskApp.Authorization.Users;
using Acme.SimpleTaskApp.Entities.CartItems;
using Acme.SimpleTaskApp.Entities.Carts;
using Acme.SimpleTaskApp.Entities.Categories;
//using Acme.SimpleTaskApp.Entities.Customers;
using Acme.SimpleTaskApp.Entities.OrderItems;
using Acme.SimpleTaskApp.Entities.Orders;
using Acme.SimpleTaskApp.Entities.Persons;
using Acme.SimpleTaskApp.Entities.Products;
using Acme.SimpleTaskApp.Entities.Tasks;
using Acme.SimpleTaskApp.MultiTenancy;
using Microsoft.EntityFrameworkCore;


namespace Acme.SimpleTaskApp.EntityFrameworkCore;

public class SimpleTaskAppDbContext : AbpZeroDbContext<Tenant, Role, User, SimpleTaskAppDbContext>
{
    /* Define a DbSet for each entity of the application */
    //public DbSet<Customer> Customers { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<Person> People { get; set; }


    public SimpleTaskAppDbContext(DbContextOptions<SimpleTaskAppDbContext> options)
        : base(options)
    {
    }

   



}
