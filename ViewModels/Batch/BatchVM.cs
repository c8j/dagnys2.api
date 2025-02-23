namespace dagnys2.api.ViewModels.Batch;

public record class BatchVM
{
    public int ID { get; init; }
    public DateOnly ManufactureDate { get; init; }
    public DateOnly ExpirationDate { get; init; }
}
