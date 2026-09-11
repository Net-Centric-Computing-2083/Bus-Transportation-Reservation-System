namespace BusReservation.Server.Models;

public class Passenger
{
    public int PassengerId { get; set; }
    public string PassengerName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
}