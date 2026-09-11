using BusReservation.Server.Data;
using Microsoft.AspNetCore.Mvc;

namespace BusReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DatabaseTestController : ControllerBase
    {
        private readonly DatabaseConnection _databaseConnection;

        public DatabaseTestController(DatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        [HttpGet]
        public IActionResult TestConnection()
        {
            try
            {
                using var connection = _databaseConnection.CreateConnection();

                connection.Open();

                return Ok(new
                {
                    message = "Database connection successful!",
                    database = connection.Database,
                    server = connection.DataSource
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Database connection failed.",
                    error = ex.Message
                });
            }
        }
    }
}