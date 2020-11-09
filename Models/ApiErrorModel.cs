namespace MVC.Models
{
    public class ApiErrorModel
    {
        public string Message { get; set; }

        public string Information { get; set; }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
