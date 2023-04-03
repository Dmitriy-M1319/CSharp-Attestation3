namespace PrinterLib;

public abstract class Printer: IPrintingDevice
{
    public int DeviceId { get; set; }
    public List<string> Documents { get; set; }
    public string PrintProcess { get; set; }

    public string Print()
    {
        string result = "Сообщение от Printer:\n";
        foreach (var document in Documents)
            result += document + "\n";
        return result;
    }

    public int GetPrintingStatus()
    {
        switch (PrintProcess)
        {
            case "ожидает":
                return 0;
            case "печатает":
                return 1;
            case "выключен":
                return -1;
            default:
                throw new Exception("Неподходящий формат данных для процесса печати");
        }
    }
    
    public void SendDocumentToPrinter(string document)
    {
        Documents = document.Split("\n").ToList();
    }

}