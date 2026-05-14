using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Cms.Data;
using Cms.Models;
using Cms.Infrastructure;


namespace Cms.Controllers;


[Route("cms")]
[ApiController]
[Authorize(Roles = BasicAuthenticationUser.ROLE_CMS)]
public class CmsController : ControllerBase
{
	#region Members
	private readonly CmsDbContext _dbContext;
	#endregion


	#region Constructor
	public CmsController(CmsDbContext dbContext)
	{
		_dbContext = dbContext;
	}
	#endregion


	#region Actions
	[HttpPost("events.json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
	public async Task<IActionResult> PostEvents([FromBody] List<EventModel> events)
	{
		//...

		return Ok();
	}
	#endregion
}