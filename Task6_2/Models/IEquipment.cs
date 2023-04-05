namespace Task6_2.Models;

public interface IEquipment
{
    public string Name { get; set; }
    public string EquipmentType { get;}

    string Work();
}