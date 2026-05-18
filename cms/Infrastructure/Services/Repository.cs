using Microsoft.EntityFrameworkCore;

using Cms.Infrastructure.Sqlite;
using Cms.Infrastructure.Entities;


namespace Cms.Infrastructure.Services;


public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity, new()
{
  #region Members
  private readonly CmsDbContext _readDbContext;
  private readonly CmsDbContext _writeDbContext;

  private readonly DbSet<TEntity> _readDbSet;
  private readonly DbSet<TEntity> _writeDbSet;
  #endregion


  #region Constructor
  public Repository(CmsDbContext readDbContext, CmsDbContext writeDbContext)
  {
    _readDbContext = readDbContext;
    _writeDbContext = writeDbContext;

    _readDbSet = _readDbContext.Set<TEntity>();
    _writeDbSet = _writeDbContext.Set<TEntity>();
  }
  #endregion


  #region Public methods
  public async Task<TEntity?> FindAsync(params object?[]? keyValues) => await _readDbSet.FindAsync(keyValues);


  public TEntity? GetSingleOrDefault(Func<TEntity, bool> predicate) => _readDbSet.AsQueryable().SingleOrDefault(predicate);


  public IQueryable<TEntity> GetAsQueryable(Func<IQueryable<TEntity>, IQueryable<TEntity>>? query)
  {
    IQueryable<TEntity> entities = _readDbSet;

    if (query is not null)
      entities = query(entities);

    return entities;
  }


  public async Task<(IEnumerable<TEntity> Entities, int Total)> SearchAsync(
    Func<IQueryable<TEntity>, IQueryable<TEntity>> query,
    int? page = 1,
    int? limit = 25)
  {
    page = Math.Max(page ?? 1, 1);
    limit = Math.Max(limit ?? 25, 1);

    IQueryable<TEntity> entities = _readDbSet.AsNoTracking();

    if (query is not null)
      entities = query(entities);

    int totalCount = await entities.CountAsync();

    entities = entities
      .Skip((page.Value - 1) * limit.Value)
      .Take(limit.Value);

    return (await entities.ToListAsync(), totalCount);
  }


  public async Task AddAsync(TEntity entity)
  {
    if (entity is null)
      return;

    _writeDbSet.Add(entity);

    await _writeDbContext.SaveChangesAsync();
  }
  

  public async Task UpdateAsync(TEntity entity)
  {
    if (entity is null)
      return;

    _writeDbSet.Update(entity);

    await _writeDbContext.SaveChangesAsync();
  }


  public async Task DeleteAsync(TEntity? entity)
  {
    if (entity is null)
      return;

    _writeDbSet.Remove(entity);

    await _writeDbContext.SaveChangesAsync();
  }

  public async Task DeleteAsync(params object?[]? keyValues) => await DeleteAsync(await _writeDbSet.FindAsync(keyValues));
  #endregion
}