namespace GoogleCalendarIntegration.Application.Abstractions
{
    public interface IGoogleAuthService
    {
        ResponseModel<string> GetAuthCode();
        Task<ResponseModel<GoogleTokenResponse>> GetTokens(string code);
        Task<ResponseModel<GoogleTokenResponse>> RefreshToken(string RefreshToken);
        Task<ResponseModel<string>> RevokeToken(string token);
    }
}