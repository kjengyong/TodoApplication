namespace Todo.API.Models.Response
{

    /// <summary> </summary>
    public class BaseResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public BaseResponse(string? message)
        {
            IsSuccess = false;
            Message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isSuccess"></param>
        /// <param name="message"></param>
        public BaseResponse(bool isSuccess = false, string? message = null)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        /// <summary> </summary>
        public bool IsSuccess { get; set; }
        /// <summary> </summary>
        public string? Message { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseResponse<T> : BaseResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        public BaseResponse(T? result) : base(true)
        {
            Result = result;
        }
        /// <summary>
        /// 
        /// </summary>
        public T? Result { get; set; }
    }
}
