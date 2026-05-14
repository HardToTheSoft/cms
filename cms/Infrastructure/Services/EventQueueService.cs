using System.Threading.Channels;
using System.Collections.Concurrent;


namespace Cms.Services;


public sealed class EventQueueService : IEventQueueService
{
  #region Members
  private readonly ConcurrentDictionary<string, (DateTimeOffset ProcessAfter, IEvent EventToProcess)> _lookupDictionary = [];

  private readonly Channel<object> _queue = Channel.CreateUnbounded<object>(new UnboundedChannelOptions
  {
    SingleReader = true
  });
  #endregion


  #region Constructor
  public EventQueueService()
  {
    _ = StartFilteringAsync();
  }
  #endregion


  #region Public methods
  public void EnqueueAsync<TEvent>(TEvent eventToDispatch, uint delayInSeconds = 0)
    where TEvent : class, IEvent, IEventHandler<TEvent>, new()
  {
    if (eventToDispatch is null)
      return;

    string? hash = null;

    if (eventToDispatch.Id is null)
      hash = Guid.NewGuid().ToString("N");
    else
      hash = eventToDispatch.Hash;

    if (string.IsNullOrWhiteSpace(hash))
      return;

    var utcNow = DateTimeOffset.UtcNow;
    var processAfter = utcNow.AddSeconds(delayInSeconds);

    if (eventToDispatch.TriggeredAt == DateTimeOffset.MinValue)
      eventToDispatch.TriggeredAt = utcNow;

    _lookupDictionary.AddOrUpdate(hash,
      (processAfter, eventToDispatch),
      (existingKey, existingValue) =>
      {
        if (eventToDispatch.TriggeredAt > existingValue.EventToProcess.TriggeredAt)
          return (processAfter, eventToDispatch);
        else
          return (processAfter, existingValue.EventToProcess);
      });
  }


  public async ValueTask<object?> DequeueAsync(CancellationToken cancellationToken)
    => await _queue.Reader.ReadAsync(cancellationToken);
  #endregion


  #region Private methods
  private async Task StartFilteringAsync()
  {
    while (true)
    {
      try
      {
        await Task.Delay(250);

        var utcNow = DateTimeOffset.UtcNow;

        var eventToProcess = _lookupDictionary
          .OrderBy(kvp => kvp.Value.ProcessAfter)
          .FirstOrDefault(kvp => kvp.Value.ProcessAfter <= utcNow);

        if (string.IsNullOrWhiteSpace(eventToProcess.Key))
          continue;

        if (_lookupDictionary.TryRemove(eventToProcess.Key, out _))
          await _queue.Writer.WriteAsync(eventToProcess.Value.EventToProcess);
      }
      catch { }
    }
  }
  #endregion
}