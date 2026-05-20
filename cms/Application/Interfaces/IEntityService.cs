using Cms.Domain;
using Cms.Application.Dto;


namespace Cms.Application.Interfaces;


public interface IEntityService
{
	Task<Entity?> FindAsync(string id);

	Task<UserEntityDto?> GetUserEntityDtoAsync(string id);

	Task<AdminEntityDto?> GetAdminEntityDtoAsync(string id);

	Task<(IEnumerable<UserEntityDto> Entities, int Total)?> QueryUserEntityDtoAsync(int? page = 1, int? limit = 25);

	Task<(IEnumerable<AdminEntityDto> Entities, int Total)?> QueryAdminEntityDtoAsync(int? page = 1, int? limit = 25);

	Task AddAsync(Entity entity);

	Task UpdateAsync(Entity entity);

	Task DeleteAsync(string id);

	Task<AdminEntityDto?> DisableAsync(string id);
}