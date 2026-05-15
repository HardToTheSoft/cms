using Cms.Services;


namespace Cms.Models;


public sealed class CmsEventModel : EventAbstract<CmsEventModel>, IEvent, IEventHandler<CmsEventModel>
{ }