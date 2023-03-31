namespace PrinterLib;

public class JetPrinter: Printer
{
    public string PrinterName { get; set; }
    public string Color { get; set; }
    public int ColorCapacity { get; set; }

    public JetPrinter(string name, string color, int initCapacity)
    {
        PrinterName = name;
        Color = color;
        if (initCapacity % 2 != 0)
            ColorCapacity = (initCapacity % 2) * 2;
        else
            ColorCapacity = initCapacity;
    }

    public bool CheckCapacity()
    {
        return Documents.Count <= ColorCapacity / 2;
    }
    
}