using humidify.Api.Data;
using humidify.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace humidify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ILogger<SensorController> _logger;

        public SensorController(AppDbContext context, ILogger<SensorController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostSensorReading([FromBody] SensorReading reading)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (reading.Timestamp == default)
            {
                reading.Timestamp = DateTime.UtcNow;
            }

            _logger.LogInformation("Received new sensor reading: Temp={temp}°C, Humidity={hum}% at {time}",
                reading.Temperature, reading.Humidity, reading.Timestamp.ToString("s"));
            _context.SensorReadings.Add(reading);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetLatestReading), reading);
        }

        [HttpGet("latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SensorReading>> GetLatestReading()
        {
            var latestReading = await _context.SensorReadings
                .OrderByDescending(r => r.Timestamp)
                .FirstOrDefaultAsync();

            if (latestReading == null)
            {
                _logger.LogWarning("no data found");
                return NotFound();
            }

            return Ok(latestReading);
        }
    }
}