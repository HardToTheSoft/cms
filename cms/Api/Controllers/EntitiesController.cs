using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Cms.Api.Authentication;

using Cms.Application.Dto;
using Cms.Application.Interfaces;


namespace Cms.Api.Controllers;


[Route("api")]
[ApiController]
[Authorize(Roles = BasicAuthenticationUser.ROLE_USER)]
public class EntitiesController : ControllerBase
{
	#region Members
	private readonly IEntityService _entities;
	#endregion


	#region Constructor
	public EntitiesController(IEntityService entities)
	{
		_entities = entities;
	}
	#endregion


	#region Actions
	[HttpGet("entities.json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<IEnumerable<object>>> SearchAsync(
		[FromQuery] int? page = 1,
		[FromQuery] int? limit = 25)
	{
		(IEnumerable<object> Entities, int Total)? queryResult;

		if (User.IsInRole(BasicAuthenticationUser.ROLE_ADMIN))
			queryResult = await _entities.QueryAdminEntityDtoAsync(page, limit);
		else
			queryResult = await _entities.QueryUserEntityDtoAsync(page, limit);

		if (queryResult is null)
			return BadRequest();

		Response.Headers.TryAdd("X-Total", queryResult.Value.Total.ToString());

		return Ok(queryResult.Value.Entities);
	}


	[HttpGet("entities/{id}.json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<object>> FindAsync([FromRoute] string id)
	{
		object? entityDto;

		if (User.IsInRole(BasicAuthenticationUser.ROLE_ADMIN))
			entityDto = await _entities.GetAdminEntityDtoAsync(id);
		else
			entityDto = await _entities.GetUserEntityDtoAsync(id);

		return entityDto is not null ? Ok(entityDto) : NotFound();
	}


	[HttpPost("entities/{id}/disable.json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
	[Authorize(Roles = BasicAuthenticationUser.ROLE_ADMIN)]
	public async Task<ActionResult<AdminEntityDto>> DisableAsync([FromRoute] string id)
	{
		var adminEntityDto = await _entities.DisableAsync(id);

		return adminEntityDto is not null ? Ok(adminEntityDto) : BadRequest();
	}
	#endregion
}