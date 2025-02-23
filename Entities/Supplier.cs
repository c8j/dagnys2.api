namespace dagnys2.api.Entities;

public record Supplier : Entity
{
    public ICollection<Ingredient> Ingredients { get; } = [];
    public ICollection<SupplierIngredient> SupplierIngredients { get; } = [];
}
