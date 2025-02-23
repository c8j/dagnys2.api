namespace dagnys2.api.Entities;

public record class Batch
{
    public int ID { get; set; }
    public DateOnly ManufactureDate { get; set; }
    public DateOnly ExpirationDate { get; set; }

    public ICollection<Product> Products { get; } = [];
    public ICollection<ProductBatch> ProductBatches { get; } = [];
}
