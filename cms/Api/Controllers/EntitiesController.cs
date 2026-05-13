using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Cms.Infrastructure;


namespace Cms.Controllers;


[Route("api")]
[ApiController]
[Authorize(Roles = BasicAuthenticationUser.ROLE_USER)]
public class EntitiesController : ControllerBase
{
	private static readonly List<Entity> Mau =
	[
			new Entity { Id = 1, Name = "Entity 1" },
				new Entity { Id = 2, Name = "Entity 2" },
				new Entity { Id = 3, Name = "Entity 3" }
	];


	[HttpGet("entities.json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]

	public async Task<ActionResult<IEnumerable<Entity>>> GetSearch(
		[FromQuery] string? searchTerm,
		[FromQuery] int page = 1,
		[FromQuery] int pageSize = 20)
	{
		//...

		return Ok(Mau);
	}


	[HttpGet("entities/{id}.json")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
	public async Task<ActionResult<Entity>> GetById([FromRoute] int id)
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
	public async Task<ActionResult<Entity>> PostDisable([FromRoute] int id)
	{
		//...

		return Ok();
	}
}


// Entity model
public class Entity
{
	public int Id { get; set; }
	public string Name { get; set; }
}