using Cms.Data;


namespace Cms.Services;


public class EntityService : IEntityService
{
	#region Members
	private readonly IRepository<Entity> _entities;
	#endregion


	#region Constructor
	public EntityService(IRepository<Entity> entities)
	{
		_entities = entities;
	}
	#endregion


	#region Public methods
	public async Task<Entity?> FindAsync(params object?[]? keyValues) => await _entities.FindAsync(keyValues);


	public async Task<Entity?> UnpublishAsync(int id)
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