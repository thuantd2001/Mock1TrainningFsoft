using System.Net;

namespace Mock1Trainning.Models.Response
{
    public class APIResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool IsSuccess { get; set; } = true;
        public List<string> ErrorMessage { get; set; }
        public object Result { get; set; }

        public APIResponse()
        {
            ErrorMessage = new List<string>();
        }
    }
}
