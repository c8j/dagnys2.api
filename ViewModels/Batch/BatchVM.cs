using dagnys2.api.ViewModels.Product;

namespace dagnys2.api.ViewModels.Batch;

public record class BatchVM
{
    public DateOnly ManufactureDate { get; init; }
    public DateOnly ExpirationDate { get; init; }
    public ICollection<BatchItemVM> Products { get; init; } = [];
}
