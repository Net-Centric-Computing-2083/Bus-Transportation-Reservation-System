using BusReservation.Server.Models;
using Microsoft.AspNetCore.Mvc;

namespace BusReservation.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusController : ControllerBase
    {
        private static readonly List<Bus> buses = new();

        [HttpGet]
        public ActionResult<IEnumerable<Bus>> GetBuses()
        {
            return Ok(buses);
        }

        [HttpGet("{id}")]
        public ActionResult<Bus> GetBus(int id)
        {
            var bus = buses.FirstOrDefault(b => b.BusId == id);

            if (bus == null)
            {
                return NotFound();
            }

            return Ok(bus);
        }

        [HttpPost]
        public ActionResult<Bus> CreateBus(Bus bus)
        {
            bus.BusId = buses.Count == 0
                ? 1
                : buses.Max(b => b.BusId) + 1;

            buses.Add(bus);

            return CreatedAtAction(
                nameof(GetBus),
                new { id = bus.BusId },
                bus
            );
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBus(int id, Bus updatedBus)
        {
            var bus = buses.FirstOrDefault(b => b.BusId == id);

            if (bus == null)
            {
                return NotFound();
            }

            bus.BusNumber = updatedBus.BusNumber;
            bus.BusName = updatedBus.BusName;
            bus.BusType = updatedBus.BusType;
            bus.TotalSeats = updatedBus.TotalSeats;
            bus.Status = updatedBus.Status;

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBus(int id)
        {
            var bus = buses.FirstOrDefault(b => b.BusId == id);

            if (bus == null)
            {
                return NotFound();
            }

            buses.Remove(bus);

            return NoContent();
        }
    }
}