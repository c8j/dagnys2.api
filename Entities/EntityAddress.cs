using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[PrimaryKey(nameof(EntityID), nameof(AddressID), nameof(AddressTypeID))]
public record EntityAddress
{
    public int EntityID { get; set; }
    public int AddressID { get; set; }
    public int AddressTypeID { get; set; }

    public Entity Entity { get; set; }
    public Address Address { get; set; }
    public AddressType AddressType { get; set; }
}
