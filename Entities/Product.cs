using Microsoft.EntityFrameworkCore;

namespace dagnys2.api.Entities;

[Index(nameof(ItemNumber), IsUnique = true)]
public class Product
{
    public int ID { get; set; }
    public string ItemNumber { get; set; }
    public decimal Price { get; set; }

    public SupplierProduct SupplierProduct { get; set; }
}
