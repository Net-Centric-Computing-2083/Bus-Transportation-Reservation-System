namespace BusReservation.API.Models
{
    public class Driver
    {
        public int DriverId { get; set; }

        public string DriverName { get; set; } = string.Empty;

        public string LicenseNumber { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string Status { get; set; } = "Available";
    }
}