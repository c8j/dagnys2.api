namespace dagnys2.api.Entities;

public record class Product
{
    public int ID { get; set; }
    public string ItemNumber { get; set; }
    public string Name { get; set; }
    public int WeightInGrams { get; set; }
    public decimal PriceKrPerUnit { get; set; }
    public int AmountInPackage { get; set; }

    public ICollection<Batch> Batches { get; } = [];
    public ICollection<ProductBatch> ProductBatches { get; } = [];
    public ICollection<OrderItem> OrderItems { get; } = [];
}
