using Cms.Data;


namespace Cms.Services;


public interface IEntityService
{
	Task<EntityEntity?> FindAsync(params object?[]? keyValues);

	Task AddAsync(EntityEntity entity);

	Task UpdateAsync(EntityEntity entity);

	Task DeleteAsync(params object?[]? keyValues);

	Task<EntityEntity?> UnpublishAsync(string id);
}