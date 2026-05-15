namespace Cms.Services;


public interface IEventHandlerService<in TEvent> where TEvent : class, IEvent, IEventHandler<TEvent>, new()
{
  Task HandleEventAsync(TEvent eventToHandle);
}