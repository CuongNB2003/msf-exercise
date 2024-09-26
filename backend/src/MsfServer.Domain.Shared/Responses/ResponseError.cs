namespace MsfServer.Domain.Shared.Responses
{
    public class ResponseError
    {
        public int? Status { get; set; }
        public string? Error { get; set; }
        public string? Instance { get; set; }
    }
}
