using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[Index(nameof(Name), IsUnique = true)]
public class AddressType
{
    public int ID { get; set; }
    public string Name { get; set; }
    public IList<SupplierAddress> SupplierAddresses { get; set; } = [];
}
