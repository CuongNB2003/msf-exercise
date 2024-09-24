namespace MsfServer.Domain.Shared.Exceptions
{
    public class CustomException(int errorCode, string errorMessage) : Exception(errorMessage)
    {
        public int ErrorCode { get; set; } = errorCode;
        public string ErrorMessage { get; set; } = errorMessage;
    }
}

