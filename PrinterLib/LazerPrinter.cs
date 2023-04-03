namespace PrinterLib;

public class LazerPrinter: Printer
{
    public string PrinterName { get; set; }
    public int PaperCount { get; set; } = 0;

    public LazerPrinter()
    {
        PrinterName = "printer1";
        PaperCount = 53;
    }
    public LazerPrinter(string name, int startPaperCount)
    {
        PrinterName = name;
        PaperCount = startPaperCount;
    }

    public bool CheckPaper()
    {
        return PaperCount <= Documents.Count;
    }

    public void AddNewPaper(int paperCount)
    {
        if (paperCount < 0)
            throw new Exception("Неправильное переданное количество бумаги");
        PaperCount += paperCount;
    }

}