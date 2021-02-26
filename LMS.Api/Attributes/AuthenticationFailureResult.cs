using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net;

namespace LMS.Api.Attributes
{
    public class AuthenticationFailureResult : IHttpActionResult
    {
        private string ReasonPhrase;
        private HttpRequestMessage Request;
        public AuthenticationFailureResult(string reasonPhrase, HttpRequestMessage request)
        {
            this.ReasonPhrase = reasonPhrase;
            this.Request = request;
        }
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            response.RequestMessage = Request;
            response.ReasonPhrase = ReasonPhrase;
            return Task.FromResult(response);
        }
    }
}