
using Cms.Data;


namespace Cms.Services;


public interface ISearchService
{
  IQueryable<TEntity> GetAsQueryable<TEntity>(Func<IQueryable<TEntity>, IQueryable<TEntity>>? query = null)
    where TEntity : class, IEntity, new();


  Task<(IEnumerable<TEntity> Entities, int Total)> QueryAsync<TEntity>(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> query,
    int? page = 1,
    int? limit = 25)
    where TEntity : class, IEntity, new();
}