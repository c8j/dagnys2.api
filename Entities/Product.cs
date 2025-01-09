namespace dagnys2.api.Entities;

public class Product
{
    public int ID { get; set; }
    public string ItemNumber { get; set; }
    public decimal Price { get; set; }

    public ProductType ProductType { get; set; }
    public Supplier Supplier { get; set; }
}
