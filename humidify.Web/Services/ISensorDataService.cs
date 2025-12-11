using humidify.Core.Models;

namespace humidify.Web.Services
{
    public interface ISensorDataService
    {
        Task<SensorReading?> GetLatestReadingAsync();
    }
}