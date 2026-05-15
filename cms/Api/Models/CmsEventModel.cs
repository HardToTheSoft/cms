using Cms.Api.Services;


namespace Cms.Api.Models;


public sealed class CmsEventModel : EventAbstract<CmsEventModel>, IEvent, IEventHandler<CmsEventModel>
{ }