using dagnys2.api.ViewModels.Entity;

namespace dagnys2.api.ViewModels.Product;

public record class ProductVM
{
    public string ItemNumber { get; init; }
    public string Name { get; init; }
    public int WeightInGrams { get; init; }
    public decimal PriceKrPerUnit { get; init; }
    public int AmountInPackage { get; init; }
    public ICollection<ProductBatchVM> Batches { get; init; }
    public ICollection<SimpleEntityVM> Customers { get; init; }
}
