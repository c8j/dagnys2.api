namespace dagnys2.api.Entities;

public class PhoneType
{
    public int ID { get; set; }
    public string Name { get; set; }

    public IList<Phone> Phones { get; set; }
}
