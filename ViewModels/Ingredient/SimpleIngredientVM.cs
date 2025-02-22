namespace dagnys2.api.ViewModels.Ingredient;

public record SimpleIngredientVM
{
    public int ID { get; init; }
    public string ItemNumber { get; init; }
    public string Name { get; init; }
}
