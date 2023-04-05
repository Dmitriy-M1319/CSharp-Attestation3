namespace Task6_2.Models;

public class SugarEquipment : IEquipment
{
    public string Name { get; set; }
    public string EquipmentType { get; }
    public int SugarCount { get; set; }

    public SugarEquipment(string name, int count)
    {
        Name = name;
        SugarCount = count;
        EquipmentType = "Sugar Producer";
    }
    public string Work()
    {
        return $"Sugar Produced is worked: {SugarCount} of sugar produced";
    }
}