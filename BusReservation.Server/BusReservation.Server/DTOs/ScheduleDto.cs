namespace BusReservation.Server.DTOs;

public class ScheduleDto
{
	public int ScheduleId { get; set; }

	public int BusId { get; set; }
	public int RouteId { get; set; }
	public int DriverId { get; set; }

	public string? BusNumber { get; set; }
	public string? Source { get; set; }
	public string? Destination { get; set; }
	public string? DriverName { get; set; }

	public DateTime DepartureDate { get; set; }

	public TimeSpan DepartureTime { get; set; }
	public TimeSpan? ArrivalTime { get; set; }

	public decimal Fare { get; set; }

	public string Status { get; set; } = "Active";
}