namespace MsfServer.Domain.Shared.Responses
{
    public class ResponseText
    {
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;

        public ResponseText() { }

        public ResponseText(int status, string message)
        {
            Status = status;
            Message = message;
        }

        public static ResponseText ResponseSuccess(string message, int status)
        {
            return new ResponseText(status, message);
        }
    }
}
