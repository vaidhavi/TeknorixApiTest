using System.Net;

namespace TekTestApi.Model
{
    public class ResultDto
    {
        public bool Result { get; set; }
        public dynamic Details { get; set; }

        public HttpStatusCode Status { get; set; }
    }
}
