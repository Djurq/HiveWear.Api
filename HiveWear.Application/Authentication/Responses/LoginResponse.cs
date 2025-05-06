namespace HiveWear.Application.Authentication.Responses
{
    public record class LoginResponse(string RefreshToken, string JwtToken, string UserName)
    {
    }
}
