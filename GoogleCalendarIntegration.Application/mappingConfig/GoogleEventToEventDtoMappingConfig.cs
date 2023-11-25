using Google.Apis.Calendar.v3.Data;
using Mapster;

namespace GoogleCalendarIntegration.Application.mappingConfig
{
    internal class GoogleEventToEventDtoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Event, GoogleCalenderEventDto>()
            .MapWith(src => new GoogleCalenderEventDto()
            {
                Attachment = src.Attachments == null ? null : src.Attachments.FirstOrDefault()!.FileUrl,
                Start = src.Start == null ? null : src.Start.DateTime,
                End = src.End == null ? null : src.End.DateTime,
                Summary = src.Summary,
                Description = src.Description,
            });
        }
    }
}
