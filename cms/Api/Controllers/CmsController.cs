using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Cms.Models;
using Cms.Services;
using Cms.Infrastructure;


namespace Cms.Controllers;


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
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> EventsAsync([FromBody] List<EventModel> events)
	{
		//nameof(events) is here for swagger only. we wish to avoid reading the body stream twice...
		Request.Body.Position = 0;

		using var streamReader = new StreamReader(Request.Body, Encoding.UTF8);

		var body = await streamReader.ReadToEndAsync();

		_eventDispatcher.Dispatch(new CmsEventModel
		{
			Topic = "events/process",
			Payload = "body"
		});

		return Ok();
	}
	#endregion
}