
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MsfServer.Domain.Entities
{
    public class RequestLog : BaseModel
    {
        [Required]
        [MaxLength(10)]
        public string? Method { get; set; }

        [Required]
        public int StatusCode { get; set; }

        [Required]
        [MaxLength(255)]
        public string? Url { get; set; }

        [Required]
        [MaxLength(15)]
        public string? ClientIpAddress { get; set; }

        [MaxLength(255)]
        public string? UserName { get; set; }

        [Required]
        public int Duration { get; set; }

        public static RequestLog AddLogEntry(string? method, int statusCode, string? url, string? clientIpAddress, string? userName, int duration)
        {
            return new RequestLog
            {
                Method = method,
                StatusCode = statusCode,
                Url = url,
                ClientIpAddress = clientIpAddress,
                UserName = userName,
                Duration = duration,
            };
        }
    }
}
