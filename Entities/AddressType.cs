namespace dagnys2.api.Entities;


public class AddressType
{
    public int ID { get; set; }
    public string Name { get; set; }
    public IList<SupplierAddress> SupplierAddresses { get; set; }
}
