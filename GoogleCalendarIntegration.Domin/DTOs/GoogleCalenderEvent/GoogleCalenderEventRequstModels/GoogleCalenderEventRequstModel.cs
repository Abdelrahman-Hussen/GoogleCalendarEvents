namespace GoogleCalendarIntegration.Domin.DTOs
{
    public class GoogleCalenderEventRequstModel : RequestModel
    {
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public string? Email { get; set; }
    }
}
