using LMS.Common.Encryption;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace LMS.Api.Attributes
{
    public class Authenticate : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            var apiKey = request.Headers.GetValues("X-API-Key").FirstOrDefault();

            if (String.IsNullOrEmpty(apiKey))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing API Key", request);
                return;
            }
            bool checkApi = await ValidateApiAsync(apiKey);
            if (!checkApi)
                context.ErrorResult = new AuthenticationFailureResult("Invalid API Key", request);
            return;
        }
        private Task<bool> ValidateApiAsync(string apiKey)
        {
            string decryptedUserApiKey = AESAlgorithm.Decrypt(apiKey, ConfigurationManager.AppSettings.Get("AESKey"));
            string actualApiKey = ConfigurationManager.AppSettings.Get("API_Key");
            if (decryptedUserApiKey == actualApiKey)
                return Task.FromResult(true);
            else
                return Task.FromResult(false);
        }
        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            if (context.Result is AuthenticationFailureResult)
            {
                var challenge = new AuthenticationHeaderValue[]
                {
                    new AuthenticationHeaderValue("API_Key")
                };
                context.Result = new UnauthorizedResult(challenge, context.Request);
                return Task.FromResult(context.Result);
            }
            else
                return Task.FromResult(0);
        }
    }
}