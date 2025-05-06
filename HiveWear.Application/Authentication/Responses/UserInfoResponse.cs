namespace HiveWear.Application.Authentication.Responses
{
    public class UserInfoResponse
    {
        public string RefreshToken { get; set; } = null!;
        public string JwtToken { get; set; } = null!;
        public string UserName { get; set; } = null!;
    }
}
