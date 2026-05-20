using Cms.Domain;

using Cms.Application.Dto;
using Cms.Application.Extensions;
using Cms.Application.Interfaces;

using Cms.Infrastructure.Persistence.Services;


namespace Cms.Application.Services;


public class EntityService : IEntityService
{
	#region Members
	private readonly IEntityRepository<Entity> _entities;

	//private readonly ISearchEntityService<Entity> _search;

	private readonly IUserContext _userContext;
	#endregion


	#region Constructor
	public EntityService(
		IEntityRepository<Entity> entities,
		/*ISearchEntityService<Entity> search,*/
		IUserContext userContext)
	{
		_entities = entities;
		//_search = search;
		_userContext = userContext;
	}
	#endregion


	#region Public methods
	public async Task<Entity?> FindAsync(string id)
		=> string.IsNullOrWhiteSpace(id) ? null : await _entities.FindAsync(id.ToLower());


	public async Task<UserEntityDto?> GetUserEntityDtoAsync(string id)
	{
		if (!_userContext.IsUser)
			return null;

		var entity = await FindAsync(id);

		return entity.ToUserEntityDto();
	}


	public async Task<AdminEntityDto?> GetAdminEntityDtoAsync(string id)
	{
		if (!_userContext.IsAdmin)
			return null;

		var entity = await FindAsync(id);

		return entity.ToAdminEntityDto();
	}


	public async Task<(IEnumerable<UserEntityDto> Entities, int Total)?> QueryUserEntityDtoAsync(int? page = 1, int? limit = 25)
	{
		if (!_userContext.IsUser)
			return null;

		// var (entities, total) = await _search.QueryAsync(query =>
		// 	{
		// 		query = query.Where(e => !e.Disabled);

		// 		return query.OrderByDescending(e => e.UpdatedAt);
		// 	}, page, limit);

		// return (entities?.Select(e => e.ToUserEntityDto()!) ?? [], total);

		throw new NotImplementedException();
	}


	public async Task<(IEnumerable<AdminEntityDto> Entities, int Total)?> QueryAdminEntityDtoAsync(int? page = 1, int? limit = 25)
	{
		if (!_userContext.IsAdmin)
			return null;

		var f = await _entities.QueryEntityAsync(page, limit);

		//var (entities, total) = await _entities.QueryEntityAsync(page, limit);

		return (f.Value.Entities.Select(e => e.ToAdminEntityDto()!) ?? [], f.Value.Total);

		// var (entities, total) = await _search.QueryAsync(query => query.OrderByDescending(e => e.UpdatedAt),
		// 	page, limit);

		// return (entities.Select(e => e.ToAdminEntityDto()!) ?? [], total);


		//throw new NotImplementedException();
	}


	public async Task AddAsync(Entity entity) => await _entities.AddAsync(entity);


	public async Task UpdateAsync(Entity entity) => await _entities.UpdateAsync(entity);


	public Task DeleteAsync(string id)
		=> string.IsNullOrWhiteSpace(id) ? Task.CompletedTask : _entities.DeleteAsync(id.ToLower());


	public async Task<AdminEntityDto?> DisableAsync(string id)
	{
		if (!_userContext.IsAdmin)
			return null;

		if (string.IsNullOrWhiteSpace(id))
			return null;

		var entity = await _entities.FindAsync(id.ToLower());

		if (entity == null)
			return null;

		if (entity.Disabled)
			return entity.ToAdminEntityDto();

		entity.Disabled = true;
		entity.UpdatedAt = DateTimeOffset.UtcNow;

		await _entities.UpdateAsync(entity);

		return entity.ToAdminEntityDto();
	}
	#endregion
}