namespace dagnys2.api.ViewModels.OrderItem;

public record class OrderItemVM
{
    public string ItemNumber { get; init; }
    public string ProductName { get; init; }
    public decimal PriceKrPerUnit { get; init; }
    public int Quantity { get; init; }
    public decimal TotalPriceKr { get; init; }

}
