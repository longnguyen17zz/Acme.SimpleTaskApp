using Abp.Zero.EntityFrameworkCore;
using Acme.SimpleTaskApp.Authorization.Roles;
using Acme.SimpleTaskApp.Authorization.Users;
using Acme.SimpleTaskApp.Entities.Banners;
using Acme.SimpleTaskApp.Entities.Batches;
using Acme.SimpleTaskApp.Entities.CartItems;
using Acme.SimpleTaskApp.Entities.Carts;
using Acme.SimpleTaskApp.Entities.Categories;
using Acme.SimpleTaskApp.Entities.Discounts;
using Acme.SimpleTaskApp.Entities.FlashSaleItems;
using Acme.SimpleTaskApp.Entities.FlashSales;
using Acme.SimpleTaskApp.Entities.HistoryViews;

//using Acme.SimpleTaskApp.Entities.Customers;
using Acme.SimpleTaskApp.Entities.OrderItems;
using Acme.SimpleTaskApp.Entities.Orders;
using Acme.SimpleTaskApp.Entities.Persons;
using Acme.SimpleTaskApp.Entities.ProductReviews;
using Acme.SimpleTaskApp.Entities.Products;
using Acme.SimpleTaskApp.Entities.Stocks;
using Acme.SimpleTaskApp.Entities.Tasks;
using Acme.SimpleTaskApp.Entities.Thumbs;
using Acme.SimpleTaskApp.Entities.Vouchers;
using Acme.SimpleTaskApp.MultiTenancy;
using Microsoft.EntityFrameworkCore;


namespace Acme.SimpleTaskApp.EntityFrameworkCore;

public class SimpleTaskAppDbContext : AbpZeroDbContext<Tenant, Role, User, SimpleTaskAppDbContext>
{
	/* Define a DbSet for each entity of the application */

	public DbSet<Banner> Banners { get; set; }
	public DbSet<HistoryView> HistoryViews { get; set; }
	public DbSet<ProductReview> ProductReviews { get; set; }

	public DbSet<FlashSale> FlashSales { get; set; }
	public DbSet<FlashSaleItem> FlashSaleItems { get; set; }
	public DbSet<Voucher> Vouchers { get; set; }
	public DbSet<Discount> Discounts { get; set; }


	public DbSet<CartItem> CartItems { get; set; }
	public DbSet<Cart> Carts { get; set; }
	public DbSet<Order> Orders { get; set; }
	public DbSet<OrderItem> OrderItems { get; set; }

	public DbSet<Category> Categories { get; set; }
	public DbSet<Product> Products { get; set; }
	public DbSet<Stock> Stocks { get; set; }
	public DbSet<Task> Tasks { get; set; }
	public DbSet<Person> People { get; set; }


	public DbSet<Thumb> Thumbs { get; set; }
	public DbSet<Batch> Batches { get; set; }
	public SimpleTaskAppDbContext(DbContextOptions<SimpleTaskAppDbContext> options)
				: base(options)
	{
	}





}
