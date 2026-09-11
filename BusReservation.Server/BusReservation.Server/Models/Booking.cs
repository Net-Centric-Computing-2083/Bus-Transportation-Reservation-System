namespace BusReservation.Server.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        public int PassengerId { get; set; }

        public int ScheduleId { get; set; }

        public int SeatId { get; set; }

        public DateTime BookingDate { get; set; }

        public string Status { get; set; } = "Confirmed";
    }
}