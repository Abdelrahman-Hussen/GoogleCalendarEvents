namespace GoogleCalendarIntegration.Domin.DTOs
{
    public class UpdateGoogleCalenderEventDto
    {
        public long Id { get; set; }
        public string? Summary { get; set; }
        public string? Description { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public IFormFile? Attachment { get; set; }
    }
}
