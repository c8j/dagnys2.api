namespace dagnys2.api.Entities;

public record Phone
{
    public int ID { get; set; }
    public string Number { get; set; }

    public ICollection<Entity> Entities { get; } = [];
    public ICollection<EntityPhone> EntityPhones { get; } = [];
}
