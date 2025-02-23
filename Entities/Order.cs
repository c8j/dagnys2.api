namespace dagnys2.api.Entities;

public record class Order
{
    public int ID { get; set; }
    public string GeneratedNumber { get; set; }
    public DateTime DateCreated { get; set; }
    public int CustomerID { get; set; }

    public Customer Customer { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }

}
