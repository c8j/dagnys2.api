using dagnys2.api.ViewModels.OrderItem;

namespace dagnys2.api.ViewModels.Order;

public record class OrderVM
{
    public int OrderNumber { get; init; }
    public DateOnly DateCreated { get; init; }
    public int CustomerID { get; init; }
    public string CustomerName { get; init; }
    public ICollection<OrderItemVM> OrderItems { get; init; }
}
