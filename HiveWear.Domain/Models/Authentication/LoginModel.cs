namespace HiveWear.Domain.Models.Authentication
{
    public sealed class LoginModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
