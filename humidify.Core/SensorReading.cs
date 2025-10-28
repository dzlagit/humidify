namespace humidify.Core
{
    public class SensorReading
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double Humidity { get; set; }
        public double Temperature { get; set; } // Use double for consistency
    }
}