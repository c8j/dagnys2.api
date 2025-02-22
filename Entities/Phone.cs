namespace dagnys2.api.Entities;

public record Phone
{
    public int ID { get; set; }
    public string Number { get; set; }

    public ICollection<Supplier> Suppliers { get; } = [];
    public ICollection<SupplierPhone> SupplierPhones { get; } = [];
}
