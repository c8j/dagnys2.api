using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[PrimaryKey(nameof(SupplierID), nameof(AddressID), nameof(AddressTypeID))]
public class SupplierAddress
{
    public int SupplierID { get; set; }
    public int AddressID { get; set; }
    public int AddressTypeID { get; set; }

    public Supplier Supplier { get; set; }
    public Address Address { get; set; }
    public AddressType AddressType { get; set; }
}
