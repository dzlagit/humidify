using humidify.Api.Data; // Your DbContext
using humidify.Core;       // Your SensorReading model
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // You need this for FirstOrDefaultAsync

namespace humidify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SensorController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/sensor/reading
        [HttpPost("reading")]
        public async Task<IActionResult> PostSensorReading([FromBody] SensorReading reading)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Set the timestamp to the current server time
                reading.Timestamp = DateTime.UtcNow;

                // Add the new reading to the database
                _context.SensorReadings.Add(reading); // Corrected: SensorReadings
                await _context.SaveChangesAsync();

                // Return a "201 Created" response
                // We use the "GetLatestReading" method name here
                return CreatedAtAction(nameof(GetLatestReading), new { id = reading.Id }, reading);
            }
            catch (Exception ex)
            {
                // Return an error if something goes wrong
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        // GET: api/sensor/latest
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatestReading()
        { // The typo was here
            var latestReading = await _context.SensorReadings
                                              .OrderByDescending(r => r.Timestamp)
                                              .FirstOrDefaultAsync();

            if (latestReading == null)
            {
                return NotFound("No readings found.");
            }

            return Ok(latestReading);
        }
    }
}