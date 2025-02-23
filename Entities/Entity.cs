using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[Index(nameof(Email), IsUnique = true)]
public abstract record Entity
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string ContactName { get; set; }
    public string Email { get; set; }

    public ICollection<Address> Addresses { get; } = [];
    public ICollection<Phone> Phones { get; } = [];
    public ICollection<EntityAddress> EntityAddresses { get; } = [];
    public ICollection<EntityPhone> EntityPhones { get; } = [];
}
