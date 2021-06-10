namespace LabirunModel.Labirun.Response
{
    public class ApiResponseMessage
    {
        public ApiResponseCode Code { get; set; }
        public string Message { get; set; }

        public ApiResponseMessage()
        {
            Code = ApiResponseCode.Ok;
        }

        public ApiResponseMessage(ApiResponseCode code, string message)
        {
            Code = code;
            Message = message;
        }

        public override string ToString()
        {
            return $"{nameof(Code)}: {Code}, {nameof(Message)}: {Message}";
        }
    }


    public class ApiResponse<T>
    {
        public T Data { get; set; }
        public ApiResponseMessage Message { get; set; }

        public static ApiResponse<T> CreateSuccess(T data)
        {
            return new ApiResponse<T>()
            {
                Data = data,
                Message = new ApiResponseMessage(),
            };
        }

        public static ApiResponse<T> CreateError(ApiResponseCode responseCode, string message = null)
        {
            return new ApiResponse<T>()
            {
                Data = default,
                Message = new ApiResponseMessage(responseCode, message)
            };
        }
    }
}