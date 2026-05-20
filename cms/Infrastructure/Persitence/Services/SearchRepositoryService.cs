using Cms.Domain;

using Cms.Application.Interfaces;

using Cms.Infrastructure.Persistence.Repositories;


namespace Cms.Infrastructure.Persistence.Services;


public sealed class SearchRepositoryService : ISearchRepositoryService<EntityRepository>
{
	#region Members
	private readonly IRepository<EntityRepository> _entities;
	#endregion


	#region Constructor
	public SearchRepositoryService(IRepository<EntityRepository> entities)
	{
		_entities = entities;
	}
	#endregion


	#region Public methods
	public IQueryable<EntityRepository> GetAsQueryable(Func<IQueryable<EntityRepository>, IQueryable<EntityRepository>>? query = null)
		=> _entities.GetAsQueryable(query);

	public async Task<(IEnumerable<EntityRepository> Entities, int Total)> QueryAsync(Func<IQueryable<EntityRepository>, IQueryable<EntityRepository>> query, int? page = 1, int? limit = 25)
		=> await _entities.SearchAsync(query, page, limit);
	#endregion
}