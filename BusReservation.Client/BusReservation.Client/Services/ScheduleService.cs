using System.Net.Http.Json;
using BusReservation.Client.Models;

namespace BusReservation.Client.Services;

public class ScheduleService
{
    private readonly HttpClient _http;

    public ScheduleService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<Schedule>> GetSchedulesAsync()
    {
        return await _http.GetFromJsonAsync<List<Schedule>>(
            "api/schedules") ?? new List<Schedule>();
    }

    public async Task<Schedule?> GetByIdAsync(int id)
    {
        return await _http.GetFromJsonAsync<Schedule>(
            $"api/schedules/{id}");
    }

    public async Task<bool> CreateAsync(Schedule schedule)
    {
        var response = await _http.PostAsJsonAsync(
            "api/schedules",
            schedule);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdateAsync(Schedule schedule)
    {
        var response = await _http.PutAsJsonAsync(
            $"api/schedules/{schedule.ScheduleId}",
            schedule);

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync(
            $"api/schedules/{id}");

        return response.IsSuccessStatusCode;
    }
}