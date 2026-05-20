using Cms.Domain;


namespace Cms.Application.Interfaces;


public interface IEntityRepository<TEntity> where TEntity : class, IEntity, new()
{
	Task<Entity?> FindAsync(string id);

	Task<(IEnumerable<Entity> Entities, int Total)?> QueryAsync(int? page = 1, int? limit = 25);

	Task AddAsync(Entity entity);

	Task UpdateAsync(Entity entity);

	Task DeleteAsync(string id);
}