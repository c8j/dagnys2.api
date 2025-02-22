namespace dagnys2.api.Entities;

public record Address
{
    public int ID { get; set; }
    public string StreetLine { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }

    public ICollection<Entity> Entities { get; } = [];
    public ICollection<EntityAddress> EntityAddresses { get; } = [];
}
