using Cms.Domain;

using Cms.Application.Dto;
using Cms.Application.Extensions;
using Cms.Application.Interfaces;


namespace Cms.Application.Services;


public class EntityService : IEntityService
{
	#region Members
	private readonly IEntityRepository<Entity> _entities;

	private readonly IUserContext _userContext;
	#endregion


	#region Constructor
	public EntityService(IEntityRepository<Entity> entities, IUserContext userContext)
	{
		_entities = entities;
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

		var entities = await _entities.QueryAsync(page, limit);

		if (entities is null)
			return null;

		return (entities.Value.Entities.Select(e => e.ToUserEntityDto()!) ?? [], entities.Value.Total);
	}


	public async Task<(IEnumerable<AdminEntityDto> Entities, int Total)?> QueryAdminEntityDtoAsync(int? page = 1, int? limit = 25)
	{
		if (!_userContext.IsAdmin)
			return null;

		var entities = await _entities.QueryAsync(page, limit);

		if (entities is null)
			return null;

		return (entities.Value.Entities.Select(e => e.ToAdminEntityDto()!) ?? [], entities.Value.Total);
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

		entity.Disable();

		await _entities.UpdateAsync(entity);

		return entity.ToAdminEntityDto();
	}
	#endregion
}