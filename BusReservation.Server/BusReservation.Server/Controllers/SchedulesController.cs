using Microsoft.AspNetCore.Mvc;
using BusReservation.Server.Data;
using BusReservation.Server.Models;

namespace BusReservation.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchedulesController : ControllerBase
{
    private readonly ScheduleRepository _repository;

    public SchedulesController(ScheduleRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var schedules = await _repository.GetAllAsync();

        return Ok(schedules);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var schedule = await _repository.GetByIdAsync(id);

        if (schedule is null)
            return NotFound();

        return Ok(schedule);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Schedule schedule)
    {
        var id = await _repository.CreateAsync(schedule);

        schedule.ScheduleId = id;

        return CreatedAtAction(
            nameof(GetById),
            new { id },
            schedule);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        Schedule schedule)
    {
        if (id != schedule.ScheduleId)
            return BadRequest();

        var result = await _repository.UpdateAsync(schedule);

        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _repository.DeleteAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}