namespace dagnys2.api.ViewModels.Order;

public record class SimpleOrderVM
{
    public int ID { get; init; }
    public int OrderNumber { get; init; }
    public DateOnly DateCreated { get; init; }
}
