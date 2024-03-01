using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace api
{
  public class MessageRedis
  {
    private readonly Redis _redis;

    public MessageRedis(IOptions<Redis> options)
    {
      _redis = options.Value;
    }

    [FunctionName("redis")]
    public async Task<IActionResult> Run(
      [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)]
      HttpRequest req,
      ILogger log)
    {
      var redisConnection = await RedisConnection.InitializeAsync(connectionString: _redis.ConnectionString);
      log.LogInformation("C# HTTP trigger function to test Redis");
      var pingResult = await redisConnection.BasicRetryAsync(async (db) => await db.ExecuteAsync("PING"));
// Simple get and put of integral data types into the cache
      string key = "Message";
      string value = "Hello! The cache is working from a .NET console app!";

      var getMessageResult = await redisConnection.BasicRetryAsync(async (db) => await db.StringGetAsync(key));

      var stringSetResult = await redisConnection.BasicRetryAsync(async (db) => await db.StringSetAsync(key, value));

      return stringSetResult ? new OkObjectResult("Successfully cached") : new OkObjectResult("Unsuccessfully cached");
    }
  }
}
