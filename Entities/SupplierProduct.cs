using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[PrimaryKey(nameof(SupplierID), nameof(ProductID), nameof(ProductTypeID))]
public class SupplierProduct
{
    public int SupplierID { get; set; }
    public int ProductID { get; set; }
    public int ProductTypeID { get; set; }

    public Supplier Supplier { get; set; }
    public Product Product { get; set; }
    public ProductType ProductType { get; set; }
}
