using Cms.Domain;

using Cms.Application.Interfaces;

using Cms.Infrastructure.Persistence.Extensions;
using Cms.Infrastructure.Persistence.Repositories;


namespace Cms.Infrastructure.Persistence.Services;


public class EntityRepositoryService : IEntityRepository<Entity>
{
	#region Members
	private readonly IRepository<EntityRepository> _entities;

	private readonly ISearchRepositoryService<EntityRepository> _search;
	#endregion


	#region Constructor
	public EntityRepositoryService(
		IRepository<EntityRepository> entities,
		ISearchRepositoryService<EntityRepository> search)
	{
		_entities = entities;
		_search = search;
	}
	#endregion


	#region Public methods
	public async Task<Entity?> FindAsync(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
			return null;

		var entity = await _entities.FindAsync(id.ToLower());

		return entity?.ToEntity();
	}


	public async Task<(IEnumerable<Entity> Entities, int Total)?> QueryEntityAsync(int? page = 1, int? limit = 25)
	{
		var (entities, total) = await _search.QueryAsync(query =>
			{
				query = query.Where(e => !e.Disabled);

				return query.OrderByDescending(e => e.UpdatedAt);
			}, page, limit);

		return (entities?.Select(e => e.ToEntity()!) ?? [], total);

		//throw new NotImplementedException();
	}


	public async Task AddAsync(Entity entity) => await _entities.AddAsync(entity.ToEntityRepository());


	public async Task UpdateAsync(Entity entity) => await _entities.UpdateAsync(entity.ToEntityRepository());


	public Task DeleteAsync(string id)
		=> string.IsNullOrWhiteSpace(id) ? Task.CompletedTask : _entities.DeleteAsync(id.ToLower());


	public async Task<Entity?> DisableAsync(string id)
	{
		if (string.IsNullOrWhiteSpace(id))
			return null;

		var entity = await _entities.FindAsync(id.ToLower());

		if (entity == null)
			return null;

		if (entity.Disabled)
			return entity.ToEntity();

		entity.Disabled = true;
		entity.UpdatedAt = DateTimeOffset.UtcNow;

		await _entities.UpdateAsync(entity);

		return entity.ToEntity();
	}
	#endregion
}