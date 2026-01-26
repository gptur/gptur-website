using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

using Element.Azure.Functions.Worker.Extensions.RoutePriority;

public class Warmup {
  private readonly ILogger<Warmup> _logger;

  public Warmup(ILoggerFactory loggerFactory) {
    _logger = loggerFactory.CreateLogger<Warmup>();
  }

  [Function(nameof(Warmup))]
  public void Run([WarmupTrigger, RoutePriority] object ignore, FunctionContext executionContext) {
    _logger.LogInformation("C# Warmup trigger function processed.");
  }
}
