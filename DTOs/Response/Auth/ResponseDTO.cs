namespace ECommerceBackend.DTOs.Response.Auth
{
    public class ResponseDTO<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public ResponseDTO() { }

        public ResponseDTO(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }
    }
}
