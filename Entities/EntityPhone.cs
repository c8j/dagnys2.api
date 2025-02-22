using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[PrimaryKey(nameof(EntityID), nameof(PhoneID), nameof(PhoneTypeID))]
public record EntityPhone
{
    public int EntityID { get; set; }
    public int PhoneID { get; set; }
    public int PhoneTypeID { get; set; }

    public Entity Entity { get; set; }
    public Phone Phone { get; set; }
    public PhoneType PhoneType { get; set; }
}
