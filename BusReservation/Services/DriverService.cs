using System.Net.Http.Json;
using BusReservation.Models;

namespace BusReservation.Services
{
    public class DriverService
    {
        private readonly HttpClient _httpClient;

        public DriverService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Driver>> GetDrivers()
        {
            return await _httpClient.GetFromJsonAsync<List<Driver>>("api/Drivers")
                   ?? new List<Driver>();
        }

        public async Task<Driver?> GetDriver(int id)
        {
            return await _httpClient.GetFromJsonAsync<Driver>($"api/Drivers/{id}");
        }

        public async Task<HttpResponseMessage> AddDriver(Driver driver)
        {
            return await _httpClient.PostAsJsonAsync("api/Drivers", driver);
        }

        public async Task<HttpResponseMessage> UpdateDriver(Driver driver)
        {
            return await _httpClient.PutAsJsonAsync(
                $"api/Drivers/{driver.DriverId}", driver);
        }

        public async Task<HttpResponseMessage> DeleteDriver(int id)
        {
            return await _httpClient.DeleteAsync($"api/Drivers/{id}");
        }
    }
}