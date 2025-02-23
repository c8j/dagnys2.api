namespace dagnys2.api.ViewModels.Product;

public record class ProductBatchVM
{
    public int ID { get; init; }
    public DateOnly ManufactureDate { get; init; }
    public DateOnly ExpirationDate { get; init; }
    public int Quantity { get; init; }
}
