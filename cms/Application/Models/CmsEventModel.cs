using Cms.Application.Interfaces;


namespace Cms.Application.Models;


public sealed class CmsEventModel : EventAbstract<CmsEventModel>, IEvent, IEventHandler<CmsEventModel>
{ }