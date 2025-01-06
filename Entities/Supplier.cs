namespace dagnys2.api.Entities;

public class Supplier
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string ContactName { get; set; }
    public string Email { get; set; }

    public IList<Address> Addresses { get; set; }
    public IList<Phone> Phones { get; set; }
    public IList<Product> Products { get; set; }
}
