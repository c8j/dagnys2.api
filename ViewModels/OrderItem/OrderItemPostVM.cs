namespace dagnys2.api.ViewModels.OrderItem;

public record class OrderItemPostVM
{
    public int ProductID { get; init; }
    public int Quantity { get; init; }
}
