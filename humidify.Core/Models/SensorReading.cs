using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace humidify.Core.Models
{
    public class SensorReading
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        [Range(0, 100)]
        public double Humidity { get; set; }

        [Required]
        [Range(-50, 60)]
        public double Temperature { get; set; }
    }
}