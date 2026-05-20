using System.Text.Json;

using Cms.Infrastructure.Persistence.Repositories;

using Cms.Domain;


namespace Cms.Infrastructure.Persistence.Extensions;


public static class EntityRepositoryExtensions
{
  public static Entity? ToEntity(this EntityRepository? sqliteEntityRepository)
  {
    if (sqliteEntityRepository is null)
      return null;

    return new Entity
    {
      Id = sqliteEntityRepository.Id,
      Version = sqliteEntityRepository.Version,
      Published = sqliteEntityRepository.Published,
      PayloadJson = sqliteEntityRepository.PayloadJson ?? JsonDocument.Parse("{}"),
      CreatedAt = sqliteEntityRepository.UpdatedAt
    };
  }
}