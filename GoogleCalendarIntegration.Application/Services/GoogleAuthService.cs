using GoogleCalendarIntegration.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text;

namespace GoogleCalendarIntegration.Application.Services
{
    internal class GoogleAuthService : IGoogleAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<GoogleAuthService> _logger;

        public GoogleAuthService(HttpClient httpClient, IConfiguration configuration, ILogger<GoogleAuthService> logger)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _logger = logger;
        }


        public ResponseModel<string> GetAuthCode()
        {
            try
            {
                string mainURL = $"https://accounts.google.com/o/oauth2/auth?redirect_" +
                    $"uri={_configuration["GoogleCalinderIntegration:redirectURL"]}" +
                    $"&prompt={_configuration["GoogleCalinderIntegration:prompt"]}" +
                    $"&response_type={_configuration["GoogleCalinderIntegration:response_type"]}" +
                    $"&client_id={_configuration["GoogleCalinderIntegration:clientID"]}" +
                    $"&scope={_configuration["GoogleCalinderIntegration:scope"]}" +
                    $"&access_type={_configuration["GoogleCalinderIntegration:access_type"]}";

                return ResponseModel<string>.Success(data: mainURL);
            }
            catch (Exception ex) { _logger.Log(LogLevel.Error, ex.ToString()); }

            return ResponseModel<string>.Error();
        }

        public async Task<ResponseModel<GoogleTokenResponse>> GetTokens(string code)
        {
            try
            {
                var clientId = _configuration["GoogleCalinderIntegration:clientID"];
                string clientSecret = _configuration["GoogleCalinderIntegration:clientSecret"];
                var redirectURL = _configuration["GoogleCalinderIntegration:redirectURL"];
                var tokenEndpoint = _configuration["GoogleCalinderIntegration:tokenEndpoint"];

                var content = new StringContent($"code={code}&redirect_uri={Uri.EscapeDataString(redirectURL)}" +
                    $"&client_id={clientId}&client_secret={clientSecret}" +
                    $"&grant_type=authorization_code"
                    , Encoding.UTF8
                    , "application/x-www-form-urlencoded");

                var response = await _httpClient.PostAsync(tokenEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return ResponseModel<GoogleTokenResponse>.Error(ResponseCodes.BadRequest, $"Failed to authenticate: {responseContent}");

                var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenResponse>(responseContent);

                return ResponseModel<GoogleTokenResponse>.Success(tokenResponse);
            }
            catch (Exception ex) { _logger.Log(LogLevel.Error, ex.ToString()); }

            return ResponseModel<GoogleTokenResponse>.Error();
        }

        public async Task<ResponseModel<GoogleTokenResponse>> RefreshToken(string RefreshToken)
        {
            try
            {
                var clientId = _configuration["GoogleCalinderIntegration:clientID"];
                string clientSecret = _configuration["GoogleCalinderIntegration:clientSecret"];
                var refreshToken = RefreshToken;
                var refresh_tokenEndpoint = _configuration["GoogleCalinderIntegration:refresh_tokenEndpoint"];

                var content = new StringContent($"client_id={clientId}&client_secret={clientSecret}" +
                    $"&grant_type=refresh_token&refresh_token={refreshToken}"
                    , Encoding.UTF8
                    , "application/x-www-form-urlencoded");

                var response = await _httpClient.PostAsync(refresh_tokenEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return ResponseModel<GoogleTokenResponse>.Error(ResponseCodes.BadRequest, $"Failed to authenticate: {responseContent}");

                var tokenResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<GoogleTokenResponse>(responseContent);

                return ResponseModel<GoogleTokenResponse>.Success(tokenResponse);
            }
            catch (Exception ex) { _logger.Log(LogLevel.Error, ex.ToString()); }

            return ResponseModel<GoogleTokenResponse>.Error();
        }

        public async Task<ResponseModel<string>> RevokeToken(string token)
        {
            try
            {
                var revoke_tokenEndpoint = _configuration["GoogleCalinderIntegration:revoke_tokenEndpoint"];

                var content = new StringContent($"token={token}"
                    , Encoding.UTF8
                    , "application/x-www-form-urlencoded");

                var response = await _httpClient.PostAsync(revoke_tokenEndpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    return ResponseModel<string>.Error(ResponseCodes.BadRequest, $"Failed to authenticate: {responseContent}");

                return ResponseModel<string>.Success();
            }
            catch (Exception ex) { _logger.Log(LogLevel.Error, ex.ToString()); }

            return ResponseModel<string>.Error();
        }
    }
}
