namespace HiveWear.Domain.Result
{
    public record class LoginResult(string RefreshToken, string JwtToken, string UserName)
    {
    }
}
