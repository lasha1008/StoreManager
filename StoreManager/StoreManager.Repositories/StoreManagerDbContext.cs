using Microsoft.EntityFrameworkCore;
using StoreManager.DTO;

namespace StoreManager.Repositories;

public class StoreManagerDbContext : DbContext
{
    public StoreManagerDbContext(DbContextOptions<StoreManagerDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Employee> Employees { get; set; } = null!;
    public DbSet<AccountDetails> AccountDetails { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<City> Cities { get; set; } = null!;
    public DbSet<Country> Countries { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;
    public DbSet<OrderDetails> OrderDetails { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>().ToTable("Employees");
        modelBuilder.Entity<Customer>().ToTable("Customers");

        modelBuilder.Entity<OrderDetails>().HasKey(od => new { od.OrderId, od.ProductId });

        modelBuilder.Entity<AccountDetails>().HasIndex(x => x.Username).IsUnique();
    }
}