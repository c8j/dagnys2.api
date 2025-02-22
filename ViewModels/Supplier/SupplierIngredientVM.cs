namespace dagnys2.api.ViewModels.Supplier;

public record class SupplierIngredientVM
{
    public int ID { get; init; }
    public string ItemNumber { get; init; }
    public string Name { get; init; }
    public decimal PriceKrPerKg { get; init; }
}
