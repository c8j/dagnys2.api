namespace dagnys2.api.Entities;

public class Product
{
    public int ID { get; set; }
    public string ItemNumber { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }

    public IList<Supplier> Suppliers { get; set; }
}
