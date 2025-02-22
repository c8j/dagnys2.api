namespace dagnys2.api.ViewModels.Supplier;

public record SimpleSupplierVM
{
    public int ID { get; init; }
    public string Name { get; init; }
    public string ContactName { get; init; }
    public string Email { get; init; }
}
