namespace Cms.Application.Interfaces;


public interface IEventQueueService
{
  void EnqueueAsync<TEvent>(TEvent eventToDispatch, uint delayInSeconds = 0)
    where TEvent : class, IEvent, IEventHandler<TEvent>, new();

  ValueTask<object?> DequeueAsync(CancellationToken cancellationToken);
}