namespace dagnys2.api.Entities;

public class Address
{
    public int ID { get; set; }
    public AddressType Type { get; set; }
    public string StreetLine { get; set; }
    public string PostalCode { get; set; }
    public string City { get; set; }
    public IList<Supplier> Suppliers { get; set; }
}
