using System.ComponentModel.DataAnnotations;

namespace BusReservation.Server.Models
{
    public class Bus
    {
        public int BusId { get; set; }

        [Required(ErrorMessage = "Bus number is required.")]
        [StringLength(20)]
        public string BusNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bus name is required.")]
        [StringLength(100)]
        public string BusName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bus type is required.")]
        [StringLength(50)]
        public string BusType { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "Total seats must be between 1 and 100.")]
        public int TotalSeats { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";
    }
}