namespace DataLayer.DTOs
{
    public class ApiErrorDTO
    {
        public ApiErrorDTO(long timeStamp, string message, int errorCode)
        {
            TimeStamp = timeStamp;
            Message = message;
            ErrorCode = errorCode;
        }

        public ApiErrorDTO(string message, int errorCode) : this(DateTimeOffset.UtcNow.ToUnixTimeSeconds(), message, errorCode)
        {

        }

        public long TimeStamp { get; }
        public string Message { get; }
        public int ErrorCode { get; }
    }
}
