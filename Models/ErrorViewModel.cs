using System;

namespace MVC.Models
{
    public class ErrorViewModel
    {
        public  int ResponseCode { get; set; }

        public string ResponseMessage { get; set; }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
