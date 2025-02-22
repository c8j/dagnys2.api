namespace dagnys2.api.ViewModels.Ingredient;

public record IngredientPricePatchVM
{
    public int SupplierID { get; set; }
    public decimal PriceKrPerKg { get; set; }
}
