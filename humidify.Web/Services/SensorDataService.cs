using humidify.Core.Models;
using System.Net.Http.Json;

namespace humidify.Web.Services
{
    public class SensorDataService : ISensorDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SensorDataService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<SensorReading?> GetLatestReadingAsync()
        {
            var client = _httpClientFactory.CreateClient("Api");

            try
            {
                var reading = await client.GetFromJsonAsync<SensorReading>("api/Sensor/latest");

                return reading;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }


}