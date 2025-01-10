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
}
