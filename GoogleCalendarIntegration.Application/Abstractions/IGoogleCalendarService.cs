namespace GoogleCalendarIntegration.Application.Abstractions
{
    public interface IGoogleCalendarService
    {
        Task<ResponseModel<GoogleCalenderEventDto>> Add(CreateGoogleCalenderEventDto createGoogleCalendarEventDto);
        Task<ResponseModel<string>> Delete(long Id);
        ResponseModel<List<GoogleCalenderEventDto>> Get(GoogleCalenderEventRequstModel requestModel);
        ResponseModel<GoogleCalenderEventDto> GetById(long Id);
        ResponseModel<GoogleCalenderEventDto> GetByIdFromGoogle(long Id);
        ResponseModel<List<GoogleCalenderEventDto>> GetFromGoogle(GoogleCalenderEventRequstModel requestModel);
        Task<ResponseModel<GoogleCalenderEventDto>> Update(UpdateGoogleCalenderEventDto updateGoogleCalendarEventDto);
    }
}