namespace PrinterLib;

public interface IPrintingDevice
{
    int DeviceId { get; set; }

    string Print();
    int GetPrintingStatus(); 
}