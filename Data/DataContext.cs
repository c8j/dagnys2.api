using dagnys2.api.Entities;

using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Address> Addresses { get; set; }
    public DbSet<AddressType> AddressTypes { get; set; }
    public DbSet<Batch> Batches { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Entity> Entities { get; set; }
    public DbSet<EntityAddress> EntityAddresses { get; set; }
    public DbSet<EntityPhone> EntityPhones { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<PhoneType> PhoneTypes { get; set; }
    public DbSet<ProductBatch> ProductBatches { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<SupplierIngredient> SupplierIngredients { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Entity>()
        .HasMany(s => s.Addresses)
        .WithMany(a => a.Entities)
        .UsingEntity<EntityAddress>();

        modelBuilder.Entity<Entity>()
        .HasMany(s => s.Phones)
        .WithMany(p => p.Entities)
        .UsingEntity<EntityPhone>();

        modelBuilder.Entity<Supplier>()
        .HasMany(s => s.Ingredients)
        .WithMany(i => i.Suppliers)
        .UsingEntity<SupplierIngredient>();

        modelBuilder.Entity<Batch>()
        .HasMany(b => b.Products)
        .WithMany(p => p.Batches)
        .UsingEntity<ProductBatch>();

    }
}
