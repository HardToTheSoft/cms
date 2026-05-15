using Microsoft.Extensions.Logging;

using System.Diagnostics;


namespace Cms.Tests.Infrastructure;



public class TestLogger<T> : ILogger<T>
{
  #region Private declarations
  private class NullScope : IDisposable
  {
    public void Dispose() { }
  }
  #endregion


  #region Members
  private readonly TestContext _testContext;
  #endregion


  #region Constructor
  public TestLogger(TestContext testContext)
  {
    _testContext = testContext ?? throw new ArgumentNullException(nameof(testContext));
  }
  #endregion


  #region Properties
  public Dictionary<string, LogEntry> Messages { get; } = new(StringComparer.OrdinalIgnoreCase);
  #endregion


  #region Public methods
  public IDisposable BeginScope<TState>(TState state) => new NullScope();


  public bool IsEnabled(LogLevel logLevel) => true;


  public void Log<TState>(
    LogLevel logLevel,
    EventId eventId,
    TState state,
    Exception? exception,
    Func<TState, Exception?, string> formatter)
  {
    string message = formatter(state, exception);

    var entry = new LogEntry
    {
      LogLevel = logLevel,
      Message = message,
      State = state,
      Exception = exception,
      EventId = eventId
    };

    string key = message;

    int counter = 1;

    while (Messages.ContainsKey(key))
      key = $"{message} ({counter++})";

    Messages[key] = entry;

    _testContext.WriteLine($"[{logLevel}] {message}");

    Debug.WriteLine($"[{logLevel}] {message}");
    Console.WriteLine($"[{logLevel}] {message}");
  }
  #endregion
}