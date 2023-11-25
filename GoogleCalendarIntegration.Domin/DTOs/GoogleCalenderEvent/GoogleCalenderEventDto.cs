namespace GoogleCalendarIntegration.Domin.DTOs
{
    public class GoogleCalenderEventDto
    {
        public long? Id { get; set; }
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public string? Attachment { get; set; }
    }
}
