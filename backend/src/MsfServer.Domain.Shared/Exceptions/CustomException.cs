namespace MsfServer.Domain.Shared.Exceptions
{
    public class CustomException(int errorCode, string errorMessage, string title = "", Exception? innerException = null) : Exception(errorMessage, innerException)
    {
        public int ErrorCode { get; set; } = errorCode;
        public string ErrorMessage { get; set; } = errorMessage;
        public string Title { get; set; } = title;
    }
}

