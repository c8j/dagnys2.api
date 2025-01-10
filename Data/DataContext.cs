using dagnys2.api.Entities;

using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Data;

public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<ProductType> ProductTypes { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<AddressType> AddressTypes { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<PhoneType> PhoneTypes { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<SupplierAddress> SupplierAddresses { get; set; }
    public DbSet<SupplierPhone> SupplierPhones { get; set; }
    public DbSet<SupplierProduct> SupplierProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SupplierAddress>()
            .HasOne(e => e.Address)
            .WithMany(e => e.SupplierAddresses)
            .HasForeignKey(e => e.AddressID);

        modelBuilder.Entity<SupplierAddress>()
            .HasOne(e => e.AddressType)
            .WithMany(e => e.SupplierAddresses)
            .HasForeignKey(e => e.AddressTypeID);

        modelBuilder.Entity<SupplierAddress>()
            .HasOne(e => e.Supplier)
            .WithMany(e => e.SupplierAddresses)
            .HasForeignKey(e => e.SupplierID);

        modelBuilder.Entity<SupplierPhone>()
            .HasOne(e => e.Phone)
            .WithMany(e => e.SupplierPhones)
            .HasForeignKey(e => e.PhoneID);

        modelBuilder.Entity<SupplierPhone>()
            .HasOne(e => e.PhoneType)
            .WithMany(e => e.SupplierPhones)
            .HasForeignKey(e => e.PhoneTypeID);

        modelBuilder.Entity<SupplierPhone>()
            .HasOne(e => e.Supplier)
            .WithMany(e => e.SupplierPhones)
            .HasForeignKey(e => e.SupplierID);

        modelBuilder.Entity<SupplierProduct>()
            .HasOne(e => e.Product)
            .WithOne(e => e.SupplierProduct)
            .HasForeignKey<SupplierProduct>(e => e.ProductID);

        modelBuilder.Entity<SupplierProduct>()
            .HasOne(e => e.ProductType)
            .WithMany(e => e.SupplierProducts)
            .HasForeignKey(e => e.ProductTypeID);

        modelBuilder.Entity<SupplierProduct>()
            .HasOne(e => e.Supplier)
            .WithMany(e => e.SupplierProducts)
            .HasForeignKey(e => e.SupplierID);
    }
}
