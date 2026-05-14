using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Cms.Models;
using Cms.Infrastructure;


namespace Cms.Controllers;


[Route("api")]
[ApiController]
[Authorize(Roles = BasicAuthenticationUser.ROLE_USER)]
public class EntitiesController : ControllerBase
{
	[HttpGet("entities.json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

	public async Task<ActionResult<IEnumerable<EntityModel>>> GetSearch(
		[FromQuery] int? page = 1,
		[FromQuery] int? limit = 25)
	{
		//...

		return Ok();
	}


	[HttpGet("entities/{id}.json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<EntityModel>> GetById([FromRoute] int id)
	{
		//...

		return Ok();
	}


	[HttpPost("entities/{id}/disable.json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
	[Authorize(Roles = BasicAuthenticationUser.ROLE_ADMIN)]
	public async Task<ActionResult<EntityModel>> PostDisable([FromRoute] int id)
	{
		//...

		return Ok();
	}
}