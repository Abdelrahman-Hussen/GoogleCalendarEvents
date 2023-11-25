using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GoogleCalendarIntegration.Infrastructure.Reposatory;
using GoogleCalendarIntegration.Domin.Models.GoogleCalendar;
using MapsterMapper;
using GoogleCalendarIntegration.Application.Utils;
using MeetingManagement.Domain.Helper;
using FluentValidation;
using GoogleCalendarIntegration.Application.Specifications;
using GoogleCalendarIntegration.Application.Abstractions;

namespace GoogleCalendarIntegration.Application.Services
{
    internal class GoogleCalendarService : IGoogleCalendarService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<GoogleCalendarEvent> _logger;
        private readonly IGenericRepository<GoogleCalendarEvent> _googleCalendarEventRepo;
        private readonly IValidator<CreateGoogleCalenderEventDto> _createGoogleCalenderEventValidation;
        private readonly IValidator<UpdateGoogleCalenderEventDto> _updateGoogleCalenderEventValidation;
        private readonly IMapper _mapper;

        public GoogleCalendarService(IConfiguration configuration, ILogger<GoogleCalendarEvent> logger,
            IValidator<CreateGoogleCalenderEventDto> createGoogleCalenderEventValidation,
            IValidator<UpdateGoogleCalenderEventDto> updateGoogleCalenderEventValidation,
            IGenericRepository<GoogleCalendarEvent> googleCalendarEventRepo, IMapper mapper)
        {
            _googleCalendarEventRepo = googleCalendarEventRepo;
            _createGoogleCalenderEventValidation = createGoogleCalenderEventValidation;
            _updateGoogleCalenderEventValidation = updateGoogleCalenderEventValidation;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
        }

        public ResponseModel<List<GoogleCalenderEventDto>> GetFromGoogle(GoogleCalenderEventRequstModel requestModel)
        {
            try
            {
                var calenderService = GetCalendarService(Helpers.GetGoogleToken());

                EventsResource.ListRequest listRequest = calenderService.Events.List(requestModel.Email);
                var Events = listRequest.Execute().Items;

                if (!String.IsNullOrEmpty(requestModel.Search))
                    Events.Where(c => c.Summary.ToLower().Contains(requestModel.Search.ToLower())
                    || c.Description.ToLower().Contains(requestModel.Search.ToLower()));

                if (requestModel.From != null)
                    Events.Where(x => x.Start.DateTime > requestModel.From);

                if (requestModel.To != null)
                    Events.Where(x => x.End.DateTime < requestModel.To);

                var GoogleCalenderEvents =  _mapper.Map<List<GoogleCalenderEventDto>>(Events);

                return ResponseModel<List<GoogleCalenderEventDto>>.Success(GoogleCalenderEvents);

            }
            catch (Exception ex) { _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, ex.ToString()); }

            return ResponseModel<List<GoogleCalenderEventDto>>.Error();
        }

        public ResponseModel<GoogleCalenderEventDto> GetByIdFromGoogle(long Id)
        {
            try
            {
                var result = _googleCalendarEventRepo.GetById(Id);

                if (result == null)
                    return ResponseModel<GoogleCalenderEventDto>.Error(ResponseCodes.NotFound, Localizer.GetLanguage() == Language.EN ? "Not Found" : "غير موجود");
                
                var calenderService = GetCalendarService(Helpers.GetGoogleToken());
                
                EventsResource.GetRequest getRequest = calenderService.Events.Get(result.CalendarIdInGoogle,result.EventIdInGoogle);
                var Event = getRequest.Execute();

                var GoogleCalenderEvent = _mapper.Map<GoogleCalenderEventDto>(Event);

                return ResponseModel<GoogleCalenderEventDto>.Success(GoogleCalenderEvent);

            }
            catch (Exception ex) { _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, ex.ToString()); }

            return ResponseModel<GoogleCalenderEventDto>.Error();
        }

        public ResponseModel<List<GoogleCalenderEventDto>> Get(GoogleCalenderEventRequstModel requestModel)
        {
            try
            {
                var result = _googleCalendarEventRepo.GetWithSpec(new GoogleCalenderEventSpecifications(requestModel));

                var GoogleCalenderEvents = _mapper.Map<List<GoogleCalenderEventDto>>(result.data);

                return ResponseModel<List<GoogleCalenderEventDto>>.Success(GoogleCalenderEvents);

            }
            catch (Exception ex) { _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, ex.ToString()); }

            return ResponseModel<List<GoogleCalenderEventDto>>.Error();
        }

        public ResponseModel<GoogleCalenderEventDto> GetById(long Id)
        {
            try
            {
                var result = _googleCalendarEventRepo.GetById(Id);

                if (result == null)
                    return ResponseModel<GoogleCalenderEventDto>.Error(ResponseCodes.NotFound,Localizer.GetLanguage() == Language.EN ? "Not Found" : "غير موجود");

                var GoogleCalenderEvent = _mapper.Map<GoogleCalenderEventDto>(result);

                return ResponseModel<GoogleCalenderEventDto>.Success(GoogleCalenderEvent);

            }
            catch (Exception ex) { _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, ex.ToString()); }

            return ResponseModel<GoogleCalenderEventDto>.Error();
        }

        public async Task<ResponseModel<GoogleCalenderEventDto>> Add(CreateGoogleCalenderEventDto createGoogleCalendarEventDto)
        {
            try
            {
                var validationResult = await _createGoogleCalenderEventValidation.ValidateAsync(createGoogleCalendarEventDto);

                if (!validationResult.IsValid)
                    return ResponseModel<GoogleCalenderEventDto>.Error(ResponseCodes.BadRequest, Helpers.ArrangeValidationErrors(validationResult.Errors));

                var googleCalenderEvent = _mapper.Map<GoogleCalendarEvent>(createGoogleCalendarEventDto);
                
                var attachments = new List<EventAttachment>();
                
                if (createGoogleCalendarEventDto.Attachment != null)
                {
                    googleCalenderEvent.UploadAttachment(createGoogleCalendarEventDto.Attachment);

                    attachments = new List<EventAttachment>()
                    {
                        new EventAttachment()
                        {
                            FileUrl =  $"{Helpers.BaseUrl()}{FileHelper.GoogleCalenderAttachment}/{googleCalenderEvent.Attachment}",
                            Title = googleCalenderEvent.Attachment,
                        }
                    };
                }

                var calenderService = GetCalendarService(Helpers.GetGoogleToken());

                Event newEvent = new Event()
                {
                    Summary = createGoogleCalendarEventDto.Summary,
                    Description = createGoogleCalendarEventDto.Description,
                    Start = new EventDateTime()
                    {
                        DateTime = createGoogleCalendarEventDto.Start,
                    },
                    End = new EventDateTime()
                    {
                        DateTime = createGoogleCalendarEventDto.End,
                    },
                    Attachments = attachments.Any() ? attachments : null
                };

                EventsResource.InsertRequest insertRequest = calenderService.Events.Insert(newEvent, createGoogleCalendarEventDto.Email);
                Event createdEvent = insertRequest.Execute();

                googleCalenderEvent.EventIdInGoogle = createdEvent.Id;

                await _googleCalendarEventRepo.Add(googleCalenderEvent);
                await _googleCalendarEventRepo.Save();

                return ResponseModel<GoogleCalenderEventDto>.Success(_mapper.Map<GoogleCalenderEventDto>(googleCalenderEvent));
            }
            catch (Exception ex) { _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, ex.ToString()); }

            return ResponseModel<GoogleCalenderEventDto>.Error();
        }

        public async Task<ResponseModel<GoogleCalenderEventDto>> Update(UpdateGoogleCalenderEventDto updateGoogleCalendarEventDto)
        {
            try
            {
                var validationResult = await _updateGoogleCalenderEventValidation.ValidateAsync(updateGoogleCalendarEventDto);

                if (!validationResult.IsValid)
                    return ResponseModel<GoogleCalenderEventDto>.Error(ResponseCodes.BadRequest, Helpers.ArrangeValidationErrors(validationResult.Errors));


                var googleCalenderEvent = _googleCalendarEventRepo.GetById(updateGoogleCalendarEventDto.Id);
                if (googleCalenderEvent == null)
                    return ResponseModel<GoogleCalenderEventDto>.Error(ResponseCodes.NotFound, "Not Found");

                googleCalenderEvent.Update(updateGoogleCalendarEventDto);
                _googleCalendarEventRepo.Update(googleCalenderEvent);

                var attachments = new List<EventAttachment>()
                {
                    new EventAttachment()
                    {
                        FileUrl =  $"{Helpers.BaseUrl()}{FileHelper.GoogleCalenderAttachment}/{googleCalenderEvent.Attachment}",
                        Title = googleCalenderEvent.Attachment,
                    }
                };

                var calenderService = GetCalendarService(Helpers.GetGoogleToken());

                Event newEvent = new Event()
                {
                    Summary = updateGoogleCalendarEventDto.Summary,
                    Description = updateGoogleCalendarEventDto.Description,
                    Start = new EventDateTime()
                    {
                        DateTime = updateGoogleCalendarEventDto.Start,
                    },
                    End = new EventDateTime()
                    {
                        DateTime = updateGoogleCalendarEventDto.End,
                    },
                    Attachments = attachments
                };
                EventsResource.UpdateRequest updateRequest = calenderService.Events.Update(newEvent, googleCalenderEvent.CalendarIdInGoogle, googleCalenderEvent.EventIdInGoogle);
                Event updatedEvent = updateRequest.Execute();

                await _googleCalendarEventRepo.Save();

                return ResponseModel<GoogleCalenderEventDto>.Success(_mapper.Map<GoogleCalenderEventDto>(googleCalenderEvent));
            }
            catch (Exception ex) { _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, ex.ToString()); }

            return ResponseModel<GoogleCalenderEventDto>.Error();
        }

        public async Task<ResponseModel<string>> Delete(long Id)
        {
            try
            {
                var googleCalenderEvent = _googleCalendarEventRepo.GetById(Id);

                if (googleCalenderEvent == null)
                    return ResponseModel<string>.Error(ResponseCodes.NotFound, message: Localizer.GetLanguage() == Language.EN ? "Not Found" : "غير موجود");

                googleCalenderEvent.DeleteAttachment();

                var calenderService = GetCalendarService(Helpers.GetGoogleToken());

                EventsResource.DeleteRequest deleteRequest = calenderService.Events.Delete(googleCalenderEvent.CalendarIdInGoogle, googleCalenderEvent.EventIdInGoogle);
                deleteRequest.Execute();

                _googleCalendarEventRepo.Delete(googleCalenderEvent);
                await _googleCalendarEventRepo.Save();

                return ResponseModel<string>.Success();
            }
            catch (Exception ex) { _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, ex.ToString()); }

            return ResponseModel<string>.Error();
        }

        private CalendarService GetCalendarService(string Token)
        {
            try
            {
                var token = new TokenResponse { RefreshToken = Token };

                var credentials = new UserCredential(new GoogleAuthorizationCodeFlow(
                  new GoogleAuthorizationCodeFlow.Initializer
                  {
                      ClientSecrets = new ClientSecrets
                      {
                          ClientId = _configuration["GoogleCalinderIntegration:clientID"],
                          ClientSecret = _configuration["GoogleCalinderIntegration:clientSecret"]
                      },
                  }), "user", token);

                var service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials,
                });

                return service;
            }
            catch (Exception ex)
            {
                _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, ex.ToString());

                throw new Exception("Error While creating Calender service", ex);
            }
        }
    }
}
