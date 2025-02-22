using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[Index(nameof(ItemNumber), IsUnique = true)]
public record Ingredient
{
    public int ID { get; set; }
    public string ItemNumber { get; set; }
    public string Name { get; set; }

    public ICollection<Supplier> Suppliers { get; }
    public ICollection<SupplierIngredient> SupplierIngredients { get; }
}
