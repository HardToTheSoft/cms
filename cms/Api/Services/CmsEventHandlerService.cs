using System.Reflection;

using Cms.Models;


namespace Cms.Services;


public class CmsEventHandlerService : EventHandlerServiceAbstract<CmsEventModel>, IEventHandlerService<CmsEventModel>
{
  #region Members
  private static Dictionary<string, MethodInfo>? _methods = null;

  private readonly ILogger<CmsEventHandlerService> _logger;
  #endregion


  #region Constructor
  public CmsEventHandlerService(ILogger<CmsEventHandlerService> logger)
  {
    _logger = logger;
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
  [Topics("events/process")]
  private async Task ProcessEventsAsync(CmsEventModel cmsEventModel)
  {
    //...
  }
  #endregion
}