using dagnys2.api.ViewModels.Address;
using dagnys2.api.ViewModels.Phone;

namespace dagnys2.api.ViewModels.Supplier;

public record SupplierVM
{
    public string Name { get; init; }
    public string ContactName { get; init; }
    public string Email { get; init; }
    public ICollection<AddressVM> Addresses { get; init; }
    public ICollection<PhoneVM> Phones { get; init; }
    public ICollection<SupplierIngredientVM> Products { get; init; }
}
