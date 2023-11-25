namespace GoogleCalendarIntegration.Domin.DTOs
{
    public class CreateGoogleCalenderEventDto
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public IFormFile? Attachment { get; set; }
        public string Email { get; set; }
    }
}