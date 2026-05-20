using System.Reflection;

using Cms.Domain;

using Cms.Application.Models;
using Cms.Application.Interfaces;

using Cms.Infrastructure.Attributes;
using Cms.Infrastructure.EventHandler;
using Cms.Infrastructure.Persistence.Repositories;


namespace Cms.Application.Services;


public class CmsEventHandlerService : EventHandlerServiceAbstract<CmsEventModel>, IEventHandlerService<CmsEventModel>
{
  #region Members
  private const string DELETE = "delete";
  private const string PUBLISH = "publish";
  private const string UNPUBLISH = "unpublish";

  public const string PROCESS_EVENTS_TOPIC = "events/process";

  private static Dictionary<string, MethodInfo>? _methods = null;

  private readonly ILogger<CmsEventHandlerService> _logger;

  private readonly IEntityService _entities;
  #endregion


  #region Constructor
  public CmsEventHandlerService(
    ILogger<CmsEventHandlerService> logger,
    IEntityService entities)
  {
    _logger = logger;
    _entities = entities;
  }
  #endregion


  #region Public methods
  public override async Task HandleEventAsync(CmsEventModel cmsEventModel)
  {
    await InitAsync(cmsEventModel);

    InitMethods(ref _methods);

    //...

    await DispatchEventAsync(_methods, cmsEventModel);
  }
  #endregion


  #region Private methods
  [Topics(PROCESS_EVENTS_TOPIC)]
  private async Task ProcessEventsAsync(CmsEventModel cmsEventModel)
  {
    try
    {
      if (cmsEventModel.Payload is not List<EventModel> events)
      {
        _logger.LogWarning("Payload is empty. Skip processing...");

        return;
      }

      var groupedEvents = events.GroupBy(e => e.Id);

      _logger.LogInformation("Processing {Count} grouped events", groupedEvents.Count());

      int numberOfProcessedEvents = 0;
      int numberOfPDeletedEvents = 0;
      int numberOfCreatedEvents = 0;
      int numberOfUpdatedEvents = 0;
      int numberOfIgnoredEvents = 0;

      Entity? entity;

      string id;

      foreach (var eventGroup in groupedEvents)
      {
        id = eventGroup.Key;

        try
        {
          ++numberOfProcessedEvents;

          _logger.LogDebug("Processing group for Id {Id} with {Count} events", id, eventGroup.Count());

          if (eventGroup.Any(e => e.Type.Equals(DELETE, StringComparison.OrdinalIgnoreCase)))
          {
            _logger.LogInformation("Delete event detected for Id {Id}. Executing delete flow.", id);

            await _entities.DeleteAsync(id);

            ++numberOfPDeletedEvents;

            _logger.LogDebug("Delete completed for Id {Id}", id);

            continue;
          }

          var latestEvent = eventGroup.Where(e => (e.Type.Equals(PUBLISH, StringComparison.OrdinalIgnoreCase)
                || e.Type.Equals(UNPUBLISH, StringComparison.OrdinalIgnoreCase))
              && e.Version.HasValue)
            .OrderByDescending(e => e.Version!.Value)
            .ThenByDescending(e => e.Timestamp)
            .FirstOrDefault();

          if (latestEvent is null)
          {
            _logger.LogWarning("No published/unpublished events with a valid Version found for Id {Id}", id);

            ++numberOfIgnoredEvents;

            continue;
          }

          _logger.LogInformation(
            "Latest event selected for Id {Id}. Type: {Type}, Version: {Version}, Timestamp: {Timestamp}",
            latestEvent.Id,
            latestEvent.Type,
            latestEvent.Version,
            latestEvent.Timestamp);

          entity = await _entities.FindAsync(id);

          if (entity is null)
          {
            _logger.LogInformation("Entity not found for Id {Id}. Creating new entity.", id);

            entity = new Entity
            {
              Id = id,
              Version = latestEvent.Version!.Value,
              Published = latestEvent.Type.Equals(PUBLISH, StringComparison.OrdinalIgnoreCase),
              PayloadJson = latestEvent.PayloadJson
            };

            await _entities.AddAsync(entity);

            ++numberOfCreatedEvents;

            _logger.LogDebug("Entity created for Id {Id} with Version {Version}", id, entity.Version);

            continue;
          }
          else if (entity.Version < latestEvent.Version!.Value)
          {
            _logger.LogInformation(
              "Updating entity for Id {Id}. Old Version: {OldVersion}, New Version: {NewVersion}",
              id,
              entity.Version,
              latestEvent.Version);

            entity.PayloadJson = latestEvent.PayloadJson;
            entity.Published = latestEvent.Type.Equals(PUBLISH, StringComparison.OrdinalIgnoreCase);

            await _entities.UpdateAsync(entity);

            ++numberOfUpdatedEvents;

            _logger.LogDebug("Entity updated for Id {Id}", id);

            continue;
          }

          _logger.LogDebug(
            "Ignoring event for Id {Id}. Current Version: {CurrentVersion}, Event Version: {EventVersion}",
            id,
            entity?.Version,
            latestEvent.Version);

          ++numberOfIgnoredEvents;
        }
        catch (Exception ex)
        {
          _logger.LogError(ex, "An error occurred while processing event for Id {Id}", id);
        }
      }

      _logger.LogInformation(
        "Finished processing events. Total: {Total}, Deleted: {Deleted}, Created: {Created}, Updated: {Updated}, Ignored: {Ignored}",
        numberOfProcessedEvents,
        numberOfPDeletedEvents,
        numberOfCreatedEvents,
        numberOfUpdatedEvents,
        numberOfIgnoredEvents);

      _logger.LogInformation("SUCCESS");
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "An error occurred while processing events.");

      _logger.LogInformation("FAIL");
    }
  }
  #endregion
}