namespace CompanyManagementSystem.Application.Common
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public T? Data { get; set; }

        public static Response<T> Ok(T? data, string message = "Success")
            => new() { Success = true, Message = message, Data = data };

        public static Response<T> Fail(string message)
            => new() { Success = false, Message = message, Data = default };
    }
}
