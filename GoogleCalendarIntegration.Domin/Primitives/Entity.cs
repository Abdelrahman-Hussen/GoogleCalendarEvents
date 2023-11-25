namespace GoogleCalendarIntegration.Domin.Primitives
{
    public class Entity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
