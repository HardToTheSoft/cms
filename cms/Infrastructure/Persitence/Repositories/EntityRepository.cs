using System.Text.Json;

using Cms.Application.Interfaces;


namespace Cms.Infrastructure.Persistence.Repositories;


public sealed class EntityRepository : IRepositoryEntity
{
  public string Id { get; set; } = default!;

  public int Version { get; set; }

  public bool Published { get; set; }
  public bool Disabled { get; set; }

  public JsonDocument? PayloadJson { get; set; }

  public DateTimeOffset CreatedAt { get; set; }
  public DateTimeOffset UpdatedAt { get; set; }
}