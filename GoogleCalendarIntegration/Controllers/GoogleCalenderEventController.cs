using GoogleCalendarIntegration.Application.Abstractions;
using GoogleCalendarIntegration.Domin.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCalendarIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleCalenderEventController : ControllerBase
    {
        private readonly IGoogleCalendarService _googleCalendarService;

        public GoogleCalenderEventController(IGoogleCalendarService googleCalendarService) 
        {
            _googleCalendarService = googleCalendarService;
        }

        [HttpGet("[action]")]
        public ActionResult<ResponseModel<List<GoogleCalenderEventDto>>> Get([FromQuery] GoogleCalenderEventRequstModel requestModel)
        {
            var response = _googleCalendarService.Get(requestModel);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("[action]")]
        public ActionResult<ResponseModel<List<GoogleCalenderEventDto>>> GetFromGoogle([FromQuery] GoogleCalenderEventRequstModel requestModel)
        {
            var response = _googleCalendarService.GetFromGoogle(requestModel);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("[action]")]
        public ActionResult<ResponseModel<List<GoogleCalenderEventDto>>> GetById(long Id)
        {
            var response = _googleCalendarService.GetById(Id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("[action]")]
        public ActionResult<ResponseModel<List<GoogleCalenderEventDto>>> GetByIdFromGoogle(long Id)
        {
            var response = _googleCalendarService.GetByIdFromGoogle(Id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult<ResponseModel<GoogleCalenderEventDto>>> Create([FromForm] CreateGoogleCalenderEventDto Dto)
        {
            var response = await _googleCalendarService.Add(Dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("[action]")]
        public async Task<ActionResult<ResponseModel<GoogleCalenderEventDto>>> Update([FromForm] UpdateGoogleCalenderEventDto Dto)
        {
            var response = await _googleCalendarService.Update(Dto);
            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult<ResponseModel<string>>> Delete(long Id)
        {
            var response = await _googleCalendarService.Delete(Id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
