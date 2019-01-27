namespace SendInBlue.Models
{
    public class ApiResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public ApiResponseData Data { get; set; }
    }
}
