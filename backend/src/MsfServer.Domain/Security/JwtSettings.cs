namespace MsfServer.Domain.Security
{
    public class JwtSettings
    {
        public string? Key { get; set; }
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int ExpireMinutes { get; set; }
        public int RefreshTokenExpirationDays { get; set; }
        public string? Subject { get; set; }
    }
}
