namespace humidify.Core
{
    public enum ActionType { HeatingOn, HeatingOff }

    public class UserAction
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public ActionType Action { get; set; }
        // public string UserId { get; set; } // Add this later if you add users
    }
}