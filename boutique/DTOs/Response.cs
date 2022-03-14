namespace boutique.DTOs
{
    public class Response
    {
        public string Status { get; set; }
        public string StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class SuccessResponse : Response
    {
        public dynamic Data { get; set; }
        public SuccessResponse(string message, string code = null, dynamic data = null)
        {
            Status = "success";
            StatusCode = code ?? "00";
            Message = message;
            Data = data;
        }
    }
}
