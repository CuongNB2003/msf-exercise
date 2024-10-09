namespace MsfServer.Application.Contracts.Token.Dto
{
    public class TokenJsonDto
    {
        public int UserId { get; set; }
        public string? RefreshToken { get; set; }
        public string? ExpirationDate { get; set; } // Chuyển đổi thành chuỗi
    }
}
