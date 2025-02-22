namespace dagnys2.api.Entities;

public record class ProductBatch
{
    public int ProductID { get; set; }
    public int BatchID { get; set; }
    public int Amount { get; set; }

    public Product Product { get; set; }
    public Batch Batch { get; set; }
}
