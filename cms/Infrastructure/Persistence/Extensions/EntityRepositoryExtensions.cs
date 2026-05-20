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
    (
      sqliteEntityRepository.Id,
      sqliteEntityRepository.Version,
      sqliteEntityRepository.Published,
      sqliteEntityRepository.Disabled,
      sqliteEntityRepository.PayloadJson ?? JsonDocument.Parse("{}"),
      sqliteEntityRepository.CreatedAt,
      sqliteEntityRepository.UpdatedAt
    );
  }
}