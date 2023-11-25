using MeetingManagement.Domain.Helper;

namespace GoogleCalendarIntegration.Domin.Models.GoogleCalendar
{
    public class GoogleCalendarEvent : EntityWithId 
    {
        public string Summary { get; set; }
        public string Description { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string? Attachment { get; set; }
        public string CalendarIdInGoogle { get; set; }
        public string EventIdInGoogle { get; set; }


        public void Update (UpdateGoogleCalenderEventDto Dto)
        {
            Summary = String.IsNullOrEmpty(Dto.Summary) ? Summary : Dto.Summary;
            Description = String.IsNullOrEmpty(Dto.Description) ? Description : Dto.Description;
            Start = Dto.Start ?? Start;
            End = Dto.End ?? End;

            if(Dto.Attachment != null) 
                UploadAttachment(Dto.Attachment);
        }

        public void UploadAttachment(IFormFile attachment)
        {
            if (Attachment != null)
                FileHelper.Delete(Attachment, FileHelper.GoogleCalenderAttachment);

            Attachment = FileHelper.Upload(attachment, FileHelper.GoogleCalenderAttachment);
        }

        public void DeleteAttachment()
            => FileHelper.Delete(Attachment, FileHelper.GoogleCalenderAttachment);
    }
}
