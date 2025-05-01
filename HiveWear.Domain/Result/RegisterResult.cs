namespace HiveWear.Domain.Result
{
    public record class RegisterResult(string RefreshToken, string JwtToken, string UserName)
    {
    }
}
