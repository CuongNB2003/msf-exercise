namespace MsfServer.HttpApi.ConfigRequests.ConfigDto
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
    }
}
