namespace BusReservation.Server.Models;

public class Seat
{
    public int SeatId { get; set; }
    public int BusId { get; set; }
    public int SeatNumber { get; set; }
    public string SeatType { get; set; } = "Regular";
}