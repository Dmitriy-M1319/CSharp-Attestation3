namespace PrinterLib;

public class NetPrinter: Printer
{
   public string IpAddress { get; set; }
   public string ConnectionType { get; set; }
   
   public bool IsConnected { get; set; }

   public NetPrinter(string ip, string connection)
   {
      IpAddress = ip;
      ConnectionType = connection;
   }

   public void Connect()
   {
      IsConnected = true;
   }
}