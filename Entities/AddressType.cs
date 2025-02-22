using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[Index(nameof(Name), IsUnique = true)]
public record AddressType
{
    public int ID { get; set; }
    public string Name { get; set; }
    public ICollection<SupplierAddress> SupplierAddresses { get; } = [];
}
