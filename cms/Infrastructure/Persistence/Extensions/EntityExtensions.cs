using System.Text.Json;

using Cms.Infrastructure.Persistence.Repositories;

using Cms.Domain;


namespace Cms.Infrastructure.Persistence.Extensions;


public static class EntityExtensions
{
  public static EntityRepository? ToEntityRepository(this Entity? entity)
  {
    if (entity is null)
      return null;

    return new EntityRepository
    {
      Id = entity.Id,
      Version = entity.Version,
      Published = entity.Published,
      Disabled = entity.Disabled,
      PayloadJson = entity.PayloadJson ?? JsonDocument.Parse("{}"),
      CreatedAt = entity.CreatedAt,
      UpdatedAt = entity.UpdatedAt
    };
  }
}