namespace dagnys2.api.ViewModels.Address;

public record class AddressVM
{
    public int ID { get; init; }
    public string Type { get; init; }
    public string StreetLine { get; init; }
    public string PostalCode { get; init; }
    public string City { get; init; }
}
