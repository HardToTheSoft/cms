namespace Cms.Api.Services;


public sealed class EventDispatcherBackgroundService : BackgroundService
{
  #region Members
  private const string INVOKE_EVENT_HANDLER_ASYNC = "InvokeEventHandlerAsync";

  private readonly IServiceProvider _serviceProvider;

  private readonly IEventQueueService _eventQueue;
  #endregion


  #region Constructor
  public EventDispatcherBackgroundService(IServiceProvider serviceProvider, IEventQueueService eventQueue)
  {
    _serviceProvider = serviceProvider;
    _eventQueue = eventQueue;
  }
  #endregion


  #region Private Methods
  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    while (!stoppingToken.IsCancellationRequested)
    {
      try
      {
        object? eventToProcess = await _eventQueue.DequeueAsync(stoppingToken);

        if (eventToProcess is null)
          continue;

        using var scope = _serviceProvider.CreateScope();

        var eventDispatcher = scope.ServiceProvider.GetService<IEventDispatcherService>();

        var eventType = eventToProcess.GetType();

        var invokeEventHandlerAsyncMethodInfo = eventDispatcher.GetType().GetMethod(INVOKE_EVENT_HANDLER_ASYNC).MakeGenericMethod(eventType);

        List<Task> tasks = [(Task)invokeEventHandlerAsyncMethodInfo.Invoke(eventDispatcher, [eventToProcess, null])];

        if (tasks.Count > 0)
          await Task.WhenAll(tasks);
      }
      catch { }
    }
  }
  #endregion
}