namespace POS_API.Helpers
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ApiResponse(T data, string message = "Request successful") {
            Success = true;
            Message = message;
            Data = data;
        }

        public ApiResponse(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
            Data = default(T);
        }

    }
}
