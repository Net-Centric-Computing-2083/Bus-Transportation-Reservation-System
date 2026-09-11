using BusReservation.Server.Data;
using BusReservation.Server.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BusReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly DatabaseConnection _databaseConnection;

        public BookingController(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        [HttpGet]
        public IActionResult GetBookings()
        {
            try
            {
                List<Booking> bookings = new List<Booking>();

                using var connection = _databaseConnection.CreateConnection();

                connection.Open();

                string query = @"
                    SELECT 
                        BookingId,
                        PassengerId,
                        ScheduleId,
                        SeatId,
                        BookingDate,
                        Status
                    FROM Bookings
                    ORDER BY BookingId DESC";

                using var command = new SqlCommand(query, connection);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Booking booking = new Booking
                    {
                        BookingId = reader.GetInt32(0),
                        PassengerId = reader.GetInt32(1),
                        ScheduleId = reader.GetInt32(2),
                        SeatId = reader.GetInt32(3),
                        BookingDate = reader.GetDateTime(4),
                        Status = reader.GetString(5)
                    };

                    bookings.Add(booking);
                }

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to retrieve bookings.",
                    error = ex.Message
                });
            }
        }
        [HttpPost]
        public IActionResult CreateBooking(Booking booking)
        {
            try
            {
                if (booking.PassengerId <= 0)
                {
                    return BadRequest(new
                    {
                        message = "PassengerId must be greater than 0."
                    });
                }

                if (booking.ScheduleId <= 0)
                {
                    return BadRequest(new
                    {
                        message = "ScheduleId must be greater than 0."
                    });
                }

                if (booking.SeatId <= 0)
                {
                    return BadRequest(new
                    {
                        message = "SeatId must be greater than 0."
                    });
                }

                if (string.IsNullOrWhiteSpace(booking.Status))
                {
                    booking.Status = "Confirmed";
                }
                using var connection = _databaseConnection.CreateConnection();

                connection.Open();

                string query = @"
            INSERT INTO Bookings
            (
                PassengerId,
                ScheduleId,
                SeatId,
                BookingDate,
                Status
            )
            VALUES
            (
                @PassengerId,
                @ScheduleId,
                @SeatId,
                GETDATE(),
                @Status
            );

            SELECT SCOPE_IDENTITY();";

                using var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@PassengerId", booking.PassengerId);
                command.Parameters.AddWithValue("@ScheduleId", booking.ScheduleId);
                command.Parameters.AddWithValue("@SeatId", booking.SeatId);
                command.Parameters.AddWithValue("@Status", booking.Status);

                int bookingId = Convert.ToInt32(command.ExecuteScalar());

                return Ok(new
                {
                    message = "Booking created successfully!",
                    bookingId = bookingId
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to create booking.",
                    error = ex.Message
                });
            }
        }
        [HttpPut("{id}/cancel")]
        public IActionResult CancelBooking(int id)
        {
            try
            {
                using var connection = _databaseConnection.CreateConnection();

                connection.Open();

                string query = @"
            UPDATE Bookings
            SET Status = 'Cancelled'
            WHERE BookingId = @BookingId
              AND Status = 'Confirmed'";

                using var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@BookingId", id);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    return NotFound(new
                    {
                        message = "Booking not found or already cancelled."
                    });
                }

                return Ok(new
                {
                    message = "Booking cancelled successfully!"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to cancel booking.",
                    error = ex.Message
                });
            }
        }
        [HttpGet("history/{passengerId}")]
        public IActionResult GetBookingHistory(int passengerId)
        {
            try
            {
                List<Booking> bookings = new List<Booking>();

                using var connection = _databaseConnection.CreateConnection();

                connection.Open();

                string query = @"
            SELECT
                BookingId,
                PassengerId,
                ScheduleId,
                SeatId,
                BookingDate,
                Status
            FROM Bookings
            WHERE PassengerId = @PassengerId
            ORDER BY BookingDate DESC";

                using var command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@PassengerId", passengerId);

                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Booking booking = new Booking
                    {
                        BookingId = reader.GetInt32(0),
                        PassengerId = reader.GetInt32(1),
                        ScheduleId = reader.GetInt32(2),
                        SeatId = reader.GetInt32(3),
                        BookingDate = reader.GetDateTime(4),
                        Status = reader.GetString(5)
                    };

                    bookings.Add(booking);
                }

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to retrieve booking history.",
                    error = ex.Message
                });
            }
        }
    }
}