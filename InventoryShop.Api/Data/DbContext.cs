namespace IS.DbContext;
using IS.Entities;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasMany(c => c.Orders)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.User.Id);

        modelBuilder.Entity<Order>()
            .HasMany(c => c.OrderItems)
            .WithOne(c => c.Order)
            .HasForeignKey(c => c.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(c => c.Product)
            .WithMany()
            .HasForeignKey(c => c.ProductId);

        base.OnModelCreating(modelBuilder);
    }




}