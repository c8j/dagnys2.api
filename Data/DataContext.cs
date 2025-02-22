using dagnys2.api.Entities;

using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<AddressType> AddressTypes { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<PhoneType> PhoneTypes { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<SupplierAddress> SupplierAddresses { get; set; }
    public DbSet<SupplierPhone> SupplierPhones { get; set; }
    public DbSet<SupplierIngredient> SupplierIngredients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Supplier>()
        .HasMany(s => s.Addresses)
        .WithMany(a => a.Suppliers)
        .UsingEntity<SupplierAddress>();

        modelBuilder.Entity<Supplier>()
        .HasMany(s => s.Phones)
        .WithMany(p => p.Suppliers)
        .UsingEntity<SupplierPhone>();

        modelBuilder.Entity<Supplier>()
        .HasMany(s => s.Ingredients)
        .WithMany(i => i.Suppliers)
        .UsingEntity<SupplierIngredient>();
    }
}
