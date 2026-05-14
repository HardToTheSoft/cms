using System.Reflection;


namespace Cms.Services;


public sealed class EventDispatcherService : IEventDispatcherService
{
  #region Members
  private readonly IServiceProvider _serviceProvider;

  private readonly IEventQueueService _eventQueue;
  #endregion


  #region Constructor
  public EventDispatcherService(IServiceProvider serviceProvider, IEventQueueService eventQueue)
  {
    _serviceProvider = serviceProvider;
    _eventQueue = eventQueue;
  }
  #endregion


  #region Public methods
  public void Dispatch<TEvent>(TEvent eventToDispatch)
    where TEvent : class, IEvent, IEventHandler<TEvent>, new()
  {
    _eventQueue.EnqueueAsync(eventToDispatch, 0);
  }

  public void Dispatch<TEvent>(TEvent eventToDispatch, uint delayInSeconds)
    where TEvent : class, IEvent, IEventHandler<TEvent>, new()
  {
    _eventQueue.EnqueueAsync(eventToDispatch, delayInSeconds);
  }

  public void Dispatch<TEvent>(TEvent eventToDispatch, Func<TEvent, Task>? eventHandler)
    where TEvent : class, IEvent, IEventHandler<TEvent>, new()
  {
    eventToDispatch.EventHandler = eventHandler;

    _eventQueue.EnqueueAsync(eventToDispatch, 0);
  }


  public async Task InvokeEventHandlerAsync<TEvent>(TEvent eventToProcess, Func<TEvent, Task>? eventHandler = null)
    where TEvent : class, IEvent, IEventHandler<TEvent>, new()
  {
    if (eventToProcess is null)
      return;

    List<Task> tasks = [];

    var eventType = typeof(TEvent);

    if (eventHandler is null)
    {
      var eventInterface = eventType.GetInterfaces()
        .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEventHandler<>));

      if (eventInterface!.GetProperty("EventHandler") is PropertyInfo eventHandlerPropertyInfo)
      {
        eventHandler = eventHandlerPropertyInfo.GetValue(eventToProcess) as Func<TEvent, Task>;

        if (eventHandler is not null)
          tasks.Add((Task)eventHandler.DynamicInvoke(eventToProcess)!);
      }
    }
    else
      tasks.Add((Task)eventHandler.DynamicInvoke(eventToProcess)!);

    var eventHandlerType = typeof(IEventHandlerService<>).MakeGenericType(eventType);

    var eventHandlers = _serviceProvider.GetServices(eventHandlerType);

    tasks.AddRange([.. eventHandlers.Select(eh => (Task)eventHandlerType.GetMethod("HandleEventAsync")!.Invoke(eh, [eventToProcess])!)]);

    if (tasks.Count > 0)
      await Task.WhenAll(tasks);
  }
  #endregion
}