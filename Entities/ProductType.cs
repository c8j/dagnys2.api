namespace dagnys2.api.Entities;


public class ProductType
{
    public int ID { get; set; }
    public string Name { get; set; }

    public IList<SupplierProduct> SupplierProducts { get; set; }
}
