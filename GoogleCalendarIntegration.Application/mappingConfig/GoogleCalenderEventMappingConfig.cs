using GoogleCalendarIntegration.Application.Utils;
using GoogleCalendarIntegration.Domin.Models.GoogleCalendar;
using Mapster;

namespace GoogleCalendarIntegration.Application.mappingConfig
{
    internal class GoogleCalenderEventMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<GoogleCalendarEvent, GoogleCalenderEventDto>()
                .Map(dest => dest.Attachment,
                    src => String.IsNullOrEmpty(src.Attachment) ? null 
                    : $"{Helpers.BaseUrl()}GoogleCalenderAttachments/{src.Attachment}");
        }
    }
}
