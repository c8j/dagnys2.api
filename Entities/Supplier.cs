using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[Index(nameof(Email), IsUnique = true)]
public record Supplier : Entity
{
    public ICollection<Ingredient> Ingredients { get; } = [];
    public ICollection<SupplierIngredient> SupplierIngredients { get; } = [];
}
