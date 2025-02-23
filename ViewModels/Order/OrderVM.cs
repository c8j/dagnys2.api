namespace dagnys2.api.ViewModels.Order;

public record class OrderVM
{
    public int ID { get; set; }
    public string OrderNumber { get; set; }
    public DateTime DateCreated { get; set; }
}
