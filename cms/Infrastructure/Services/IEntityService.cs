using Cms.Infrastructure.Entities;


namespace Cms.Infrastructure.Services;


public interface IEntityService
{
	Task<Entity?> FindAsync(params object?[]? keyValues);

	Task AddAsync(Entity entity);

	Task UpdateAsync(Entity entity);

	Task DeleteAsync(params object?[]? keyValues);

	Task<Entity?> UnpublishAsync(string id);
}