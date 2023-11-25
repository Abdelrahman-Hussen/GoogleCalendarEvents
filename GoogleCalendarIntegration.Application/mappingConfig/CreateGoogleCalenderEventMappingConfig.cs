using GoogleCalendarIntegration.Domin.Models.GoogleCalendar;
using Mapster;

namespace GoogleCalendarIntegration.Application.mappingConfig
{
    internal class CreateGoogleCalenderEventMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateGoogleCalenderEventDto, GoogleCalendarEvent>()
                .Map(dest => dest.CalendarIdInGoogle,
                    src => src.Email);
        }
    }
}
