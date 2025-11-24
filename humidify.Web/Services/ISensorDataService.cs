using humidify.Core.Models;

namespace humidify.Web.Services
{
    // Interface defines the contract for the service that the Home.razor component uses.
    public interface ISensorDataService
    {
        // Method to fetch the single most recent sensor reading.
        Task<SensorReading?> GetLatestReadingAsync();
    }
}