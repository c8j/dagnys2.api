namespace dagnys2.api.ViewModels.Product;

public record class BatchItemVM
{
    public int ProductID { get; init; }
    public string ItemNumber { get; init; }
    public int Quantity { get; init; }

}
