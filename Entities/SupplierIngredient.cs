using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[PrimaryKey(nameof(SupplierID), nameof(IngredientID))]
public record SupplierIngredient
{
    public int SupplierID { get; set; }
    public int IngredientID { get; set; }
    public decimal PriceKrPerKg { get; set; }

    public Supplier Supplier { get; set; }
    public Ingredient Ingredient { get; set; }
}
