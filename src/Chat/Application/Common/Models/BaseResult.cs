namespace Application.Common.Models
{
    public class BaseResult<T>
    {
        internal BaseResult(bool succeeded, T data, string message)
        {
            Succeeded = succeeded;
            Data = data;
            Message = message;
        }

        public bool Succeeded { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }

        public static BaseResult<T> Success(T data, string message)
        {
            return new BaseResult<T>(true, data, message);
        }

        public static BaseResult<T> Failure(T data, string message)
        {
            return new BaseResult<T>(false, data, message);
        }
    }
}
