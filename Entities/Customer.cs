namespace dagnys2.api.Entities;

public record class Customer : Entity
{
    public ICollection<Order> Orders { get; set; }
}
