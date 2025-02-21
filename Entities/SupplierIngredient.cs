using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[PrimaryKey(nameof(SupplierID), nameof(IngredientID), nameof(IngredientTypeID))]
public class SupplierIngredient
{
    public int SupplierID { get; set; }
    public int IngredientID { get; set; }
    public int IngredientTypeID { get; set; }

    public Supplier Supplier { get; set; }
    public Ingredient Ingredient { get; set; }
    public IngredientType IngredientType { get; set; }
}
