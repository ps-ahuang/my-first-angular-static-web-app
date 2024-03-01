using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace api
{
    public class Message
    {
      private readonly HttpClient _client;

      public Message(IHttpClientFactory httpClientFactory)
      {
        _client = httpClientFactory.CreateClient();;
      }

      [FunctionName("message")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var httpRequestMessage = new HttpRequestMessage(
              HttpMethod.Get,
              "https://api.github.com/repos/dotnet/AspNetCore.Docs/branches")
            {
              Headers =
              {
                { HeaderNames.Accept, "application/vnd.github.v3+json" },
                { HeaderNames.UserAgent, "HttpRequestsSample" }
              }
            };

            var response = await _client.SendAsync(httpRequestMessage);
            var code = response.StatusCode.ToString();

            dynamic responseMessage = new {Text = $"Hello from 'message' endpoint with status code: {code}"};

            return new OkObjectResult(responseMessage);
        }
    }
}
