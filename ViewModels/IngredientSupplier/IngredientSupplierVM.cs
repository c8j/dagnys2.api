namespace dagnys2.api.ViewModels.IngredientSupplier;

public record class IngredientSupplierVM
{
    public int SupplierID { get; init; }
    public string SupplierName { get; init; }
    public decimal Price { get; init; }
}
