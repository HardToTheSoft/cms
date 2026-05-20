// using Cms.Domain;


// namespace Cms.Application.Interfaces;


// public interface ISearchEntityService<TEntity> where TEntity : class, IEntity, new()
// {
//   IQueryable<TEntity> GetAsQueryable(Func<IQueryable<TEntity>, IQueryable<TEntity>>? query = null);

//   Task<(IEnumerable<TEntity> Entities, int Total)> QueryAsync(
//     Func<IQueryable<TEntity>, IQueryable<TEntity>> query,
//     int? page = 1, int? limit = 25);
// }