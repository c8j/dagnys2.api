using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[Index(nameof(Name), IsUnique = true)]
public record PhoneType
{
    public int ID { get; set; }
    public string Name { get; set; }

    public ICollection<SupplierPhone> SupplierPhones { get; } = [];
}
