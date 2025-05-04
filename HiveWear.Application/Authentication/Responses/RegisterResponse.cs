namespace HiveWear.Application.Authentication.Responses
{
    public record class RegisterResponse(string RefreshToken, string JwtToken, string UserName)
    {
    }
}
