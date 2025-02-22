namespace dagnys2.api.ViewModels.IngredientSupplier;

public record IngredientSuppliersPatchVM
{
    public ICollection<IngredientSuppliersVM> SuppliersVMs { get; set; } = [];
}
