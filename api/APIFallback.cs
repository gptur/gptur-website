using System.Net;
using System.Text.Json;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

public class APIFallback {
  private readonly ILogger<APIFallback> _logger;

  public APIFallback(ILoggerFactory loggerFactory) {
    _logger = loggerFactory.CreateLogger<APIFallback>();
  }

  [Function(nameof(APIFallback))]
  public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "head", "post", "put", "patch", "delete", "options", Route = "{*path}")] HttpRequestData req, string? path, FunctionContext executionContext) {
    // If we got here, no other /api route matched. (Catch-all should lose to more specific routes by normal routing precedence.)

    var method = req.Method?.ToUpperInvariant() ?? "GET";
    _logger.LogInformation("API fallback hit: {Method} /api/{Path}", method, path ?? "");

    if (method is "GET" or "HEAD") {
      // Return the SAME HTML as your site 404 page. Remember to put a copy of 404.html in your Functions output
      var response = req.CreateResponse(HttpStatusCode.NotFound);
      response.Headers.Add("Content-Type", "text/html; charset=utf-8");

      // Read 404 page from the function app file system
      var html = await Load404HtmlAsync();
      if (method != "HEAD") {
        await response.WriteStringAsync(html);
      }

      return response;
    } else {
      // JSON for non-GET/HEAD verbs
      var response = req.CreateResponse(HttpStatusCode.NotFound);
      response.Headers.Add("Content-Type", "application/json; charset=utf-8");

      var payload = new {
        error = "NotFound",
        message = "No API route matched the request.",
        method,
        path = $"/api/{path ?? ""}"
      };

      await response.WriteStringAsync(JsonSerializer.Serialize(payload));
      return response;
    }
  }

  private static async Task<string> Load404HtmlAsync() {
    var filePath = Path.Combine(AppContext.BaseDirectory, "404.html");
    return File.Exists(filePath) ? await File.ReadAllTextAsync(filePath) : "<!doctype html><html><head><meta charset='utf-8'><title>404</title></head><body><h1>404</h1><p>Not found</p></body></html>";
  }
}
