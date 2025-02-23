namespace dagnys2.api.ViewModels.Product;

public record class ProductPostVM
{
    public string ItemNumber { get; init; }
    public string Name { get; init; }
    public int WeightInGrams { get; init; }
    public decimal PriceKrPerUnit { get; init; }
    public int AmountInPackage { get; init; }
}
