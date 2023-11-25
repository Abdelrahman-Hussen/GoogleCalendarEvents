namespace GoogleCalendarIntegration.Domin.DTOs
{
    public class ResponseModel<T>
    {
        public int StatusCode { get; set; }
        public bool Ok { get; set; }
        public string Message { get; set; }
        public int TotalCount { get; set; } = 0;
        public T Data { get; set; }

        public static ResponseModel<T> Success(T data)
        {
            return new ResponseModel<T>
            {
                StatusCode = (int)ResponseCodes.Success,
                Data = data,
                Ok = true,
                Message = "success",
            };
        }
        public static ResponseModel<T> Success(T data, int totalCount)
        {
            return new ResponseModel<T>
            {
                StatusCode = (int)ResponseCodes.Success,
                Data = data,
                Ok = true,
                Message = "success",
                TotalCount = totalCount
            };
        }
        public static ResponseModel<T> Success(string message = "success")
        {
            return new ResponseModel<T>
            {
                StatusCode = (int)ResponseCodes.Success,
                Data = default(T),
                Ok = true,
                Message = message,
            };
        }
        public static ResponseModel<T> Success(string message, T data)
        {
            return new ResponseModel<T>
            {
                StatusCode = (int)ResponseCodes.Success,
                Data = data,
                Ok = true,
                Message = message,
            };
        }
        public static ResponseModel<T> Error(ResponseCodes? statusCode = ResponseCodes.InternalServerError, string message = "an error occured")
        {
            return new ResponseModel<T>
            {
                StatusCode = (int)statusCode,
                Data = default(T),
                Ok = false,
                Message = message
            };
        }
        public static ResponseModel<T> Error(T data, ResponseCodes? statusCode = ResponseCodes.InternalServerError)
        {
            return new ResponseModel<T>
            {
                StatusCode = (int)statusCode,
                Data = data,
                Ok = false,
                Message = "an error occured",
            };
        }
        public static ResponseModel<T> Error(string message, T data, ResponseCodes? statusCode = ResponseCodes.InternalServerError)
        {
            return new ResponseModel<T>
            {
                StatusCode = (int)statusCode,
                Data = data,
                Ok = false,
                Message = message
            };
        }
    }
}