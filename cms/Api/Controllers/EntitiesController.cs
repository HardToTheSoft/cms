using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Cms.Data;
using Cms.Api.Dto;
using Cms.Services;
using Cms.Domain.Extensions;
using Cms.Api.Authentication;


namespace Cms.Api.Controllers;


[Route("api")]
[ApiController]
[Authorize(Roles = BasicAuthenticationUser.ROLE_USER)]
public class EntitiesController : ControllerBase
{
	#region Members
	private readonly ISearchService _search;

	private readonly IEntityService _entities;
	#endregion


	#region Constructor
	public EntitiesController(ISearchService search, IEntityService entities)
	{
		_search = search;
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
	public async Task<ActionResult<IEnumerable<EntityDto>>> SearchAsync(
		[FromQuery] int? page = 1,
		[FromQuery] int? limit = 25)
	{
		var (entities, total) = await _search.QueryAsync<Entity>(query =>
		{
			if (!User.IsInRole(BasicAuthenticationUser.ROLE_ADMIN))
				query = query.Where(e => e.Published && !e.Disabled);

			return query.OrderByDescending(e => e.UpdatedAt);
		}, page, limit);

		Response.Headers.TryAdd("X-Total", total.ToString());

		return Ok(entities.Select(e => e.ToDto()));
	}


	[HttpGet("entities/{id}.json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<EntityDto>> FindAsync([FromRoute] string id)
	{
		var entity = await _entities.FindAsync(id);

		if (!User.IsInRole(BasicAuthenticationUser.ROLE_ADMIN))
			if (entity is null || !entity.Published || entity.Disabled)
				return NotFound();

		return entity is not null ? Ok(entity.ToDto()) : NotFound();
	}


	[HttpPost("entities/{id}/disable.json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
	[Authorize(Roles = BasicAuthenticationUser.ROLE_ADMIN)]
	public async Task<ActionResult<EntityDto>> UnpublishAsync([FromRoute] string id)
	{
		var entity = await _entities.UnpublishAsync(id);

		return entity is not null ? Ok(entity.ToDto()) : BadRequest();
	}
	#endregion
}