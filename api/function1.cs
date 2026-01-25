using System.Net;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class function1 {
  private readonly ILogger<function1> _logger;

  public function1(ILoggerFactory loggerFactory) {
    _logger = loggerFactory.CreateLogger<function1>();
  }

  [Function(nameof(function1))]
  public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = nameof(function1))] HttpRequestData req, string? path, FunctionContext executionContext) {
    _logger.LogInformation("C# HTTP trigger function processed a request.");
    var response = req.CreateResponse(HttpStatusCode.OK);
    await response.WriteStringAsync("Welcome to Azure Functions!");
    return response;
  }
}
