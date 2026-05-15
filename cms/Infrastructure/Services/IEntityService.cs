using Cms.Data;


namespace Cms.Services;


public interface IEntityService
{
	Task<Entity?> FindAsync(params object?[]? keyValues);

	Task<Entity?> UnpublishAsync(string id);
}