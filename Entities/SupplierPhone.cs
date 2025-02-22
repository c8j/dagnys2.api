using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[PrimaryKey(nameof(SupplierID), nameof(PhoneID), nameof(PhoneTypeID))]
public record SupplierPhone
{
    public int SupplierID { get; set; }
    public int PhoneID { get; set; }
    public int PhoneTypeID { get; set; }

    public Supplier Supplier { get; set; }
    public Phone Phone { get; set; }
    public PhoneType PhoneType { get; set; }
}
