using System.Net;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class f2 {
  private readonly ILogger<f2> _logger;

  public f2(ILoggerFactory loggerFactory) {
    _logger = loggerFactory.CreateLogger<f2>();
  }


  [Function(nameof(f2))]
  public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = nameof(f2))] HttpRequestData req, FunctionContext executionContext) {
    _logger.LogInformation("C# HTTP trigger function processed a request.");

    // Query string parsing (e.g. ?p1=azure&p2=2)
    string? p1 = req.Query["p1"];
    string? p2Raw = req.Query["p2"];

    int p2 = 1;
    if (!string.IsNullOrWhiteSpace(p2Raw) && int.TryParse(p2Raw, out var parsed)) {
      p2 = parsed;
    }

    var response = req.CreateResponse(HttpStatusCode.OK);
    response.Headers.Add("Content-Type", "application/json; charset=utf-8");

    response.WriteString($$"""
      {
        "p1": "{{p1}}",
        "p2": {{p2}}
      }
    """);

    return response;
  }
}