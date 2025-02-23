using dagnys2.api.ViewModels.OrderItem;

namespace dagnys2.api.ViewModels.Order;

public record class OrderPostVM
{
    public int CustomerID { get; init; }
    public ICollection<OrderItemPostVM> OrderItems { get; init; }
}
