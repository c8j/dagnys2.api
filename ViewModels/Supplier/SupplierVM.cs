using dagnys2.api.ViewModels.Entity;

namespace dagnys2.api.ViewModels.Supplier;

public record SupplierVM : EntityVM
{
    public ICollection<SupplierIngredientVM> Products { get; init; }
}
