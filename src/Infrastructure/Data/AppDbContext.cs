using Microsoft.EntityFrameworkCore;
using PathbitChallenge.Domain.Entities;
using System.Linq;

namespace PathbitChallenge.Infrastructure.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<Order> Orders { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      // Mapear as tabelas explicitamente com nomes minúsculos (conforme no PostgreSQL)
      modelBuilder.Entity<User>().ToTable("users");
      modelBuilder.Entity<Customer>().ToTable("customers");
      modelBuilder.Entity<Product>().ToTable("products");
      modelBuilder.Entity<Order>().ToTable("orders");

      // Define um padrão para colunas de string
      foreach (var property in modelBuilder.Model.GetEntityTypes()
          .SelectMany(e => e.GetProperties()
          .Where(p => p.ClrType == typeof(string))))
      {
        property.SetColumnType("varchar(255)");
      }

      // Configuração da entidade User
      modelBuilder.Entity<User>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.Email).IsUnique();
        entity.HasIndex(e => e.Username).IsUnique();
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.Username).HasColumnName("username");
        entity.Property(e => e.Email).HasColumnName("email");
        entity.Property(e => e.PasswordHash)
                    .HasColumnName("password_hash")  // underline e minúsculo
                    .HasColumnType("varchar(255)")
                    .IsRequired();
        entity.Property(e => e.UserType)
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .HasColumnName("usertype");
      });

      // Configuração da entidade Customer
      modelBuilder.Entity<Customer>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.HasIndex(e => e.Email).IsUnique();
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(e => e.Email).HasColumnName("email");
      });

      // Configuração da entidade Product
      modelBuilder.Entity<Product>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.Name).HasColumnName("name");
        entity.Property(p => p.Price)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("price");
        entity.Property(p => p.AvailableQuantity)
                    .HasColumnName("availablequantity");
      });

      // Configuração da entidade Order
      modelBuilder.Entity<Order>(entity =>
      {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Id).HasColumnName("id");
        entity.Property(e => e.Status)
                    .HasConversion<string>()
                    .HasMaxLength(50)
                    .HasColumnName("status");
        entity.Property(o => o.TotalPrice)
                    .HasColumnType("decimal(18,2)")
                    .HasColumnName("totalprice");
        entity.Property(o => o.CustomerId).HasColumnName("customerid");
        entity.Property(o => o.ProductId).HasColumnName("productid");
        entity.HasOne(o => o.Customer)
                    .WithMany(c => c.Orders)
                    .HasForeignKey(o => o.CustomerId);
        entity.HasOne(o => o.Product)
                    .WithMany()
                    .HasForeignKey(o => o.ProductId);
      });
    }
  }
}
