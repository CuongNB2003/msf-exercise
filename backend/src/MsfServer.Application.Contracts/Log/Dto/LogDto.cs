
namespace MsfServer.Application.Contracts.Log.Dto
{
    public class LogDto
    {
        public int Id { get; set; }
        public string? Method { get; set; }
        public int StatusCode { get; set; }
        public string? Url { get; set; }
        public string? ClientIpAddress { get; set; }
        public string? UserName { get; set; }
        public int Duration { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Total { get; set; }
    }
}
