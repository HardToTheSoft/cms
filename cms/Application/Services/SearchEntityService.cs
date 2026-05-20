// using Cms.Domain;

// using Cms.Application.Interfaces;
// using Cms.Infrastructure.Persistence.Repositories;


// namespace Cms.Infrastructure.Persistence.Services;


// public sealed class SearchEntityService : ISearchEntityService<Entity>
// {
//   #region Members
//   private readonly ISearchRepositoryService<EntityRepository> _search;
//   #endregion


//   #region Constructor
//   public SearchEntityService(ISearchRepositoryService<EntityRepository> search)
//   {
//     //_search = search;
//   }
//   #endregion


//   #region Public methods
//   public IQueryable<Entity> GetAsQueryable(Func<IQueryable<Entity>, IQueryable<Entity>>? query = null)
//     => _search.GetAsQueryable(query);


//   public async Task<(IEnumerable<Entity> Entities, int Total)> QueryAsync(
//     Func<IQueryable<Entity>, IQueryable<Entity>> query,
//     int? page = 1, int? limit = 25)
//       => await _search.QueryAsync(query, page, limit);
//   #endregion
// }