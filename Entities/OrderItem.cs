using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[PrimaryKey(nameof(OrderID), nameof(ProductID))]
public record class OrderItem
{
    public int OrderID { get; set; }
    public int ProductID { get; set; }
    public int Quantity { get; set; }

    public Order Order { get; set; }
    public Product Product { get; set; }
}
