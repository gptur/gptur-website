using System.Net;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class f1 {
  private readonly ILogger<f1> _logger;

  public f1(ILoggerFactory loggerFactory) {
    _logger = loggerFactory.CreateLogger<f1>();
  }

  [Function(nameof(f1))]
  public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = nameof(f1))] HttpRequestData req, FunctionContext executionContext) {
    _logger.LogInformation("C# HTTP trigger function processed a request.");
    var response = req.CreateResponse(HttpStatusCode.OK);
    await response.WriteStringAsync("Welcome to Azure Functions!");
    return response;
  }
}
