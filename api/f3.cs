using System.Net;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

// Parse route arguments (GET /api/.../{param})

public class f3 {
  private readonly ILogger<f3> _logger;

  public f3(ILoggerFactory loggerFactory) {
    _logger = loggerFactory.CreateLogger<f3>();
  }

  [Function(nameof(f3))]
  public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = nameof(f3)+"/{p1}/{p2:int?}")] HttpRequestData req, string p1, int? p2, FunctionContext executionContext) {
    _logger.LogInformation("C# HTTP trigger function processed a request.");

    var response = req.CreateResponse(HttpStatusCode.OK);
    response.Headers.Add("Content-Type", "application/json; charset=utf-8");

    response.WriteString($$"""
      {
        "p1": "{{p1}}",
        "p2": {{(p2.HasValue ? p2.Value.ToString() : "null")}}
      }
    """);

    return response;
  }
}
