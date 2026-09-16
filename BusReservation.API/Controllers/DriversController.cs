using BusReservation.API.Data;
using BusReservation.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly DatabaseHelper _databaseHelper;

        public DriversController(DatabaseHelper databaseHelper)
        {
            _databaseHelper = databaseHelper;
        }

        [HttpGet]
        public IActionResult GetAllDrivers()
        {
            try
            {
                var drivers = _databaseHelper.GetAllDrivers();

                return Ok(drivers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error retrieving drivers.",
                    error = ex.Message
                });
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetDriver(int id)
        {
            try
            {
                var driver = _databaseHelper.GetDriverById(id);

                if (driver == null)
                {
                    return NotFound(new
                    {
                        message = "Driver not found."
                    });
                }

                return Ok(driver);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error retrieving driver.",
                    error = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult AddDriver([FromBody] Driver driver)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(driver.DriverName))
                    return BadRequest(new { message = "Driver name is required." });

                if (string.IsNullOrWhiteSpace(driver.LicenseNumber))
                    return BadRequest(new { message = "License number is required." });

                if (string.IsNullOrWhiteSpace(driver.Phone))
                    return BadRequest(new { message = "Phone number is required." });

                driver.Status = string.IsNullOrWhiteSpace(driver.Status)
                    ? "Available"
                    : driver.Status;

                int result = _databaseHelper.AddDriver(driver);

                if (result > 0)
                {
                    return Ok(new
                    {
                        message = "Driver added successfully."
                    });
                }

                return BadRequest(new
                {
                    message = "Driver could not be added."
                });
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    return Conflict(new
                    {
                        message = "License number already exists."
                    });
                }

                return StatusCode(500, new
                {
                    message = "Database error occurred.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error adding driver.",
                    error = ex.Message
                });
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateDriver(int id, [FromBody] Driver driver)
        {
            try
            {
                if (id != driver.DriverId)
                {
                    return BadRequest(new
                    {
                        message = "Driver ID does not match."
                    });
                }

                if (string.IsNullOrWhiteSpace(driver.DriverName))
                    return BadRequest(new { message = "Driver name is required." });

                if (string.IsNullOrWhiteSpace(driver.LicenseNumber))
                    return BadRequest(new { message = "License number is required." });

                if (string.IsNullOrWhiteSpace(driver.Phone))
                    return BadRequest(new { message = "Phone number is required." });

                int result = _databaseHelper.UpdateDriver(driver);

                if (result == 0)
                {
                    return NotFound(new
                    {
                        message = "Driver not found."
                    });
                }

                return Ok(new
                {
                    message = "Driver updated successfully."
                });
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    return Conflict(new
                    {
                        message = "License number already exists."
                    });
                }

                return StatusCode(500, new
                {
                    message = "Database error occurred.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error updating driver.",
                    error = ex.Message
                });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteDriver(int id)
        {
            try
            {
                int result = _databaseHelper.DeleteDriver(id);

                if (result == 0)
                {
                    return NotFound(new
                    {
                        message = "Driver not found."
                    });
                }

                return Ok(new
                {
                    message = "Driver deleted successfully."
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Error deleting driver.",
                    error = ex.Message
                });
            }
        }
    }
}