using System.ComponentModel.DataAnnotations;

namespace MVC.Components.Helpers
{
    public interface IErrorMessage {
        [Required]
        string ErrorType { get; set; }
        [Required]
        int ErrorCode { get; set; }
        string GetMessage();
    }
      public class ErrorMessage : IErrorMessage
    //public class ErrorMessage
    {
        public string ErrorType { get; set; }
        public int ErrorCode { get; set; }
        public string GetMessage ()
        {
            var message = "Not implempented!";
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
            return (message);
        }
    }
}