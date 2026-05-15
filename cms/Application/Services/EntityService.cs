using Cms.Data;


namespace Cms.Services;


public class EntityService : IEntityService
{
	#region Members
	private readonly IRepository<EntityEntity> _entities;
	#endregion


	#region Constructor
	public EntityService(IRepository<EntityEntity> entities)
	{
		_entities = entities;
	}
	#endregion


	#region Public methods
	public async Task<EntityEntity?> FindAsync(params object?[]? keyValues) => await _entities.FindAsync(keyValues);


	public async Task AddAsync(EntityEntity entity) => await _entities.AddAsync(entity);


	public async Task UpdateAsync(EntityEntity entity) => await _entities.UpdateAsync(entity);


	public async Task DeleteAsync(params object?[]? keyValues) => await _entities.DeleteAsync(keyValues);


	public async Task<EntityEntity?> UnpublishAsync(string id)
	{
		var entity = await _entities.FindAsync(id);

		if (entity == null || entity.Disabled)
			return entity;

		entity.Disabled = true;
		entity.UpdatedAt = DateTimeOffset.UtcNow;

		await _entities.UpdateAsync(entity);

		return entity;
	}
	#endregion
}