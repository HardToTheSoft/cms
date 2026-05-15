using Microsoft.Extensions.Logging;


namespace Cms.Tests.Infrastructure;


public record LogEntry
{
  public LogLevel LogLevel { get; init; }

  public string Message { get; init; } = string.Empty;

  public object? State { get; init; }

  public Exception? Exception { get; init; }

  public EventId EventId { get; init; }
}