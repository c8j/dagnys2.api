using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[Index(nameof(Email), IsUnique = true)]
public class Supplier
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string ContactName { get; set; }
    public string Email { get; set; }

    public IList<SupplierAddress> SupplierAddresses { get; set; }
    public IList<SupplierPhone> SupplierPhones { get; set; }
    public IList<SupplierProduct> SupplierProducts { get; set; }
}
