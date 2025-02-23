namespace dagnys2.api.ViewModels.Address;

public record class AddressPostVM
{
    public int TypeID { get; init; }
    public string StreetLine { get; init; }
    public string PostalCode { get; init; }
    public string City { get; init; }
}
