using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Cms.Api.Dto;
using Cms.Domain.Extensions;
using Cms.Api.Authentication;
using Cms.Infrastructure.Entities;
using Cms.Infrastructure.Services;


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
	public async Task<ActionResult<IEnumerable<object>>> SearchAsync(
		[FromQuery] int? page = 1,
		[FromQuery] int? limit = 25)
	{
		bool isAdmin = true;

		var (entities, total) = await _search.QueryAsync<Entity>(query =>
		{
			if (!User.IsInRole(BasicAuthenticationUser.ROLE_ADMIN))
			{
				isAdmin = false;

				query = query.Where(e => e.Published && !e.Disabled);
			}

			return query.OrderByDescending(e => e.UpdatedAt);
		}, page, limit);

		Response.Headers.TryAdd("X-Total", total.ToString());

		if (isAdmin)
			return Ok(entities.Select(e => e.ToAdminEntityDto()));
		else
			return Ok(entities.Select(e => e.ToUserEntityDto()));
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
		var entity = await _entities.FindAsync(id);

		bool isAdmin = true;

		if (!User.IsInRole(BasicAuthenticationUser.ROLE_ADMIN))
		{
			isAdmin = false;

			if (entity is null || !entity.Published || entity.Disabled)
				return NotFound();
		}

		return entity is not null ? Ok(isAdmin ? entity.ToAdminEntityDto() : entity.ToUserEntityDto()) : NotFound();
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
		var entity = await _entities.DisableAsync(id);

		return entity is not null ? Ok(entity.ToAdminEntityDto()) : BadRequest();
	}
	#endregion
}