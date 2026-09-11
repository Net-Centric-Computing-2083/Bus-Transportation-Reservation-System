namespace BusReservation.Server.Models;

public class Bus
{
    public int BusId { get; set; }
    public string BusNumber { get; set; } = string.Empty;
    public int Capacity { get; set; }
    public string BusType { get; set; } = "Standard";
}