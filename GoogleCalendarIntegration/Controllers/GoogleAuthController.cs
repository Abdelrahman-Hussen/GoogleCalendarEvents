using GoogleCalendarIntegration.Application.Abstractions;
using GoogleCalendarIntegration.Domin.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GoogleCalendarIntegration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoogleAuthController : ControllerBase
    {
        private readonly IGoogleAuthService _googleAuthService;

        public GoogleAuthController(IGoogleAuthService googleAuthService)
        {
            _googleAuthService = googleAuthService;
        }

        [HttpGet("[action]")]
        public ActionResult<ResponseModel<string>> GetUrl()
        {
            var response = _googleAuthService.GetAuthCode();
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ResponseModel<GoogleTokenResponse>>> Callback()
        {
            var response = await _googleAuthService.GetTokens(HttpContext.Request.Query["code"]);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ResponseModel<GoogleTokenResponse>>> RefreshToken(string refreshToken)
        {
            var response = await _googleAuthService.RefreshToken(refreshToken);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<ResponseModel<GoogleTokenResponse>>> RevokeToken(string revoketoken)
        {
            var response = await _googleAuthService.RevokeToken(revoketoken);
            return StatusCode(response.StatusCode, response);
        }
    }
}
