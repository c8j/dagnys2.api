namespace dagnys2.api.Entities;

public record class Order
{
    public int ID { get; set; }
    public int GeneratedNumber { get; set; }
    public DateOnly DateCreated { get; set; }
    public int CustomerID { get; set; }

    public Customer Customer { get; set; }
    public ICollection<OrderItem> OrderItems { get; init; } = [];

}
