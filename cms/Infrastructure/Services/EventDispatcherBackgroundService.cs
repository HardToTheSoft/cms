namespace Cms.Services;


public sealed class EventDispatcherBackgroundService : BackgroundService
{
  #region Members
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

        var invokeEventHandlerAsyncMethodInfo = eventDispatcher.GetType().GetMethod("InvokeEventHandlerAsync").MakeGenericMethod(eventType);

        List<Task> tasks = [(Task)invokeEventHandlerAsyncMethodInfo.Invoke(eventDispatcher, [eventToProcess, null])];

        if (tasks.Count > 0)
          await Task.WhenAll(tasks);
      }
      catch { }
    }
  }
  #endregion
}