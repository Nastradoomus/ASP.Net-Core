namespace MVC.Components.Helpers
{
    public interface IErrorMessage {
        string GetMessage();
    }
    public class ErrorMessage : IErrorMessage
    {
        public string ErrorType { get; set; }
        public int ErrorCode { get; set; }
        public string GetMessage ()
        {
            var message = "Not implempented!";
            if (this.ErrorType == "Request") {
                switch (this.ErrorCode)
                {
                    case 200:
                        message = "OK";
                    break;
                    case 400:
                        message = "Bad Request";
                    break;
                    case 401:
                        message = "Unauthorized";
                    break;
                    case 403:
                        message = "Forbidden";
                    break;
                    case 404:
                        message = "Not Found";
                    break;
                    case 405:
                        message = "Method Not Allowed";
                    break;
                    case 406:
                        message = "Not Acceptable";
                    break;
                }
            }
            return message;
        }
    }
}