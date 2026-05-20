namespace Cms.Application.Interfaces;


public interface IRepository<TEntity> where TEntity : class, IRepositoryEntity, new()
{
  Task<TEntity?> FindAsync(params object?[]? keyValues);

  TEntity? GetSingleOrDefault(Func<TEntity, bool> predicate);

  IQueryable<TEntity> GetAsQueryable(Func<IQueryable<TEntity>, IQueryable<TEntity>>? query);

  Task<(IEnumerable<TEntity> Entities, int Total)> SearchAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> query,
    int? page = 1,
    int? limit = 25);

  Task AddAsync(TEntity? entity);

  Task UpdateAsync(TEntity? entity);

  Task DeleteAsync(TEntity? entity);

  Task DeleteAsync(params object?[]? keyValues);
}