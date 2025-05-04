namespace HiveWear.Domain.Constants
{
    public static class JwtConstants
    {
        public const string SecretKey = "Jwt:SecretKey";
        public const string Issuer = "Jwt:Issuer";
        public const string Audience = "Jwt:Audience";
        public static TimeSpan Expiration = TimeSpan.FromMinutes(30);
    }
}
