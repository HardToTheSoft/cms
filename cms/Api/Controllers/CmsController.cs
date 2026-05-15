using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Cms.Models;
using Cms.Services;
using Cms.Api.Authentication;


namespace Cms.Api.Controllers;


[Route("cms")]
[ApiController]
[Authorize(Roles = BasicAuthenticationUser.ROLE_CMS)]
public class CmsController : ControllerBase
{
	#region Members
	private readonly IEventDispatcherService _eventDispatcher;
	#endregion


	#region Constructor
	public CmsController(IEventDispatcherService eventDispatcher)
	{
		_eventDispatcher = eventDispatcher;
	}
	#endregion


	#region Actions
	[HttpPost("events.json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status415UnsupportedMediaType)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> EventsAsync([FromBody] List<EventModel> events)
	{
		_eventDispatcher.Dispatch(new CmsEventModel
		{
			Topic = "events/process",
			Payload = events
		});

		return Ok();
	}
	#endregion
}