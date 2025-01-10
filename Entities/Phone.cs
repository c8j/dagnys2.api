namespace dagnys2.api.Entities;

public class Phone
{
    public int ID { get; set; }
    public string Number { get; set; }

    public IList<SupplierPhone> SupplierPhones { get; set; } = [];
}
