using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MovieAPIDemo.Server.Models
{
    // Generic Response Class
    public class BaseResponseModel
    {
        // If operation was successful or not return Status
        public bool Status { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
