using dagnys2.api.ViewModels.Entity;
using dagnys2.api.ViewModels.Order;

namespace dagnys2.api.ViewModels.Customer;

public record class CustomerVM : EntityVM
{
    public ICollection<OrderVM> Orders { get; init; }
}
