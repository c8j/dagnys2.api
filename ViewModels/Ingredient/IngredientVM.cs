using dagnys2.api.ViewModels.IngredientSupplier;

namespace dagnys2.api.ViewModels.Ingredient;

public record IngredientVM
{
    public string ItemNumber { get; init; }
    public string Name { get; init; }

    public ICollection<IngredientSupplierVM> Suppliers { get; init; }
}
