using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[Index(nameof(Email), IsUnique = true)]
public record Supplier
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string ContactName { get; set; }
    public string Email { get; set; }

    public ICollection<Address> Addresses { get; } = [];
    public ICollection<Phone> Phones { get; } = [];
    public ICollection<Ingredient> Ingredients { get; } = [];
    public ICollection<SupplierAddress> SupplierAddresses { get; } = [];
    public ICollection<SupplierPhone> SupplierPhones { get; } = [];
    public ICollection<SupplierIngredient> SupplierIngredients { get; } = [];
}
