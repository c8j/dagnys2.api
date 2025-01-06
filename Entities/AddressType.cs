namespace dagnys2.api.Entities;

public class AddressType
{
    public int ID { get; set; }
    public string Name { get; set; }
    public IList<Address> Addresses { get; set; }
}
