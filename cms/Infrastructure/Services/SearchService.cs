using Cms.Infrastructure.Entities;


namespace Cms.Infrastructure.Services;


public sealed class SearchService : ISearchService
{
  #region Members
  private readonly IServiceProvider _serviceProvider;
  #endregion


  #region Constructor
  public SearchService(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }
  #endregion


  #region Public methods
  public IQueryable<TEntity> GetAsQueryable<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>>? query = null)
    where TEntity : class, IEntity, new()
  {
    var repository = _serviceProvider.GetRequiredService<IRepository<TEntity>>();

    return repository.GetAsQueryable(query);
  }


  public async Task<(IEnumerable<TEntity> Entities, int Total)> QueryAsync<TEntity>(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> query,
    int? page = 1,
    int? limit = 25)
    where TEntity : class, IEntity, new()
  {
    var repository = _serviceProvider.GetRequiredService<IRepository<TEntity>>();

    return await repository.SearchAsync(query, page, limit);
  }
  #endregion
}